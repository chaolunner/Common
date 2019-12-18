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
        private AsyncReceive asyncReceive;
        private AsyncReceive kcpAsyncReceive;
        private const int conv = 1;
        private const string KcpSendError = "kcp send error";

        public KcpSession(AsyncReceive asyncReceive)
        {
            this.asyncReceive = asyncReceive;
            kcpAsyncReceive = new AsyncReceive(new Message(), ReceiveCallback);

            kcpCallback = new KcpCallback();
            kcp = new Kcp(conv, kcpCallback);
            kcp.NoDelay(1, 10, 2, 1);
            kcp.WndSize(64, 64);
            kcp.SetMtu(512);

            kcpCallback.OnReceive += OnReceive;
            kcpCallback.OnOutput += OnOutput;
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
            if (!IsConnected) { return; }

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
        }

        private void BeginReceive()
        {
            if (IsConnected)
            {
                socket.BeginReceive(kcpAsyncReceive.Buffer, kcpAsyncReceive.Offset, kcpAsyncReceive.Size, SocketFlags.None, ReceiveCallback, null);
            }
            else
            {
                kcpAsyncReceive.EndReceive(0);
            }
        }

        private void ReceiveCallback(int count)
        {
            if (count > 0)
            {
                kcp.Input(kcpAsyncReceive.Buffer);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                int count = socket.EndReceive(ar);
                kcpAsyncReceive.EndReceive(count);
                BeginReceive();
            }
            catch
            {
                kcpAsyncReceive.EndReceive(0);
            }
        }

        public override bool IsConnected
        {
            get { return socket != null && kcp != null && socket.Connected; }
        }

        public override Socket Bind(IPEndPoint ipEndPoint)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(ipEndPoint);
            return socket;
        }

        public override void Send(byte[] buffer)
        {
            if (IsConnected)
            {
                if (kcp.Send(buffer) != 0)
                {
                    ConsoleUtility.WriteLine(KcpSendError, ConsoleColor.Red);
                }
            }
        }

        public override void Receive()
        {
            BeginReceive();

            Task.Run(async () =>
            {
                try
                {
                    while (IsConnected)
                    {
                        Update();
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
