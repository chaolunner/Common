using System.Net.Sockets.Kcp;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Buffers;
using System.Net;
using System;

namespace Common
{
    class KcpCallback : IKcpCallback
    {
        public event Action<byte[]> OnReceive;
        public event Action<Memory<byte>> OnOutput;

        public void Receive(byte[] buffer)
        {
            OnReceive?.Invoke(buffer);
        }

        public IMemoryOwner<byte> RentBuffer(int length)
        {
            return null;
        }

        public void Output(IMemoryOwner<byte> buffer, int avalidLength)
        {
            using (buffer)
            {
                OnOutput?.Invoke(buffer.Memory.Slice(0, avalidLength));
            }
        }
    }

    public class KcpSession : SessionBase
    {
        private Kcp kcp;
        private KcpCallback kcpCallback;
        private Socket socket;
        private IAsyncReceive asyncReceive;
        private const int conv = 1;
        private const string KcpSendError = "kcp send error";

        public KcpSession(Socket socket, IAsyncReceive asyncReceive, EndPoint remoteEP)
        {
            this.socket = socket;
            this.socket.Connect(remoteEP);
            this.asyncReceive = asyncReceive;

            kcpCallback = new KcpCallback();
            kcp = new Kcp(conv, kcpCallback);
            kcp.NoDelay(1, 10, 2, 1);
            kcp.WndSize(64, 64);
            kcp.SetMtu(512);

            kcpCallback.OnReceive += OnReceive;
            kcpCallback.OnOutput += OnOutput;

            Update();
        }

        private void OnReceive(byte[] buffer)
        {
            int count = Math.Min(asyncReceive.Size, buffer.Length);
            for (int i = 0; i < count; i++)
            {
                asyncReceive.Buffer[asyncReceive.Offset + i] = buffer[i];
            }
            asyncReceive.EndReceive(count);
            Send(buffer);
        }

        private void OnOutput(Memory<byte> buffer)
        {
            if (IsConnected)
            {
                socket.Send(buffer.ToArray());
            }
        }

        private void Update()
        {
            Task.Run(async () =>
            {
                try
                {
                    while (IsConnected)
                    {
                        kcp.Update(DateTime.UtcNow);

                        int len;
                        do
                        {
                            var (buffer, avalidLength) = kcp.TryRecv();
                            len = avalidLength;
                            if (buffer != null)
                            {
                                var avalidData = new byte[len];
                                buffer.Memory.Span.Slice(0, len).CopyTo(avalidData);
                                kcpCallback.Receive(avalidData);
                            }
                        } while (len > 0);
                        await Task.Delay(5);
                    }
                    Close();
                }
                catch (Exception e)
                {
                    ConsoleUtility.WriteLine(e);
                }
            });
        }

        public override bool IsConnected
        {
            get { return socket != null && kcp != null && socket.Connected; }
        }

        public override void Send(byte[] buffer)
        {
            if (IsConnected && kcp.Send(buffer) != 0)
            {
                ConsoleUtility.WriteLine(KcpSendError, ConsoleColor.Red);
            }
        }

        public override void Receive(IAsyncReceive asyncReceive)
        {
            if (asyncReceive != null)
            {
                kcp.Input(asyncReceive.Buffer);
            }
            else
            {
                asyncReceive.EndReceive(0);
            }
        }

        public override void Close()
        {
            if (socket != null)
            {
                socket.Close();
                socket = null;
            }

            if (kcp != null)
            {
                kcp.Dispose();
                kcp = null;
            }

            kcpCallback.OnReceive -= OnReceive;
            kcpCallback.OnOutput -= OnOutput;
        }
    }
}
