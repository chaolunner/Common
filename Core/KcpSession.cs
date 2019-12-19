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
        private bool isConnected;
        private Kcp kcp;
        private KcpCallback kcpCallback;
        private Socket socket;
        private IAsyncReceive asyncReceive;
        private EndPoint remoteEP;
        private const int conv = 1;
        private const string KcpSendError = "kcp send error";

        public KcpSession(Socket socket, IAsyncReceive asyncReceive, EndPoint remoteEP)
        {
            this.socket = socket;
            this.asyncReceive = asyncReceive;
            this.remoteEP = remoteEP;

            kcpCallback = new KcpCallback();
            kcp = new Kcp(conv, kcpCallback);
            kcp.NoDelay(1, 10, 2, 1);
            kcp.WndSize(64, 64);
            kcp.SetMtu(512);

            kcpCallback.OnReceive += OnReceive;
            kcpCallback.OnOutput += OnOutput;

            isConnected = true;

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
        }

        private void OnOutput(Memory<byte> buffer)
        {
            if (IsConnected)
            {
                socket.SendTo(buffer.ToArray(), remoteEP);
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
            get { return isConnected; }
        }

        public override void Send(byte[] buffer)
        {
            if (IsConnected && kcp.Send(buffer) != 0)
            {
                ConsoleUtility.WriteLine(KcpSendError, ConsoleColor.Red);
            }
        }

        public override void Receive(IAsyncReceive asyncReceive, int count)
        {
            byte[] buffer = new byte[count];
            for (int i = 0; i < count; i++)
            {
                buffer[i] = asyncReceive.Buffer[i];
            }
            kcp.Input(buffer);
        }

        public override void Close()
        {
            if (socket != null)
            {
                socket = null;
            }

            if (kcp != null)
            {
                kcp.Dispose();
                kcp = null;
            }

            kcpCallback.OnReceive -= OnReceive;
            kcpCallback.OnOutput -= OnOutput;

            isConnected = false;
        }
    }
}
