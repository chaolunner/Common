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
        private KcpCallback kcpCallback;
        private AsyncReceive asyncReceive;
        private Kcp kcp;
        private Socket socket;
        private const int conv = 0;

        public KcpSession(Socket socket, AsyncReceive asyncReceive)
        {
            Connect(socket.RemoteEndPoint);
            this.asyncReceive = asyncReceive;
        }

        public void Connect(EndPoint endPoint)
        {
            kcpCallback = new KcpCallback();
            kcp = new Kcp(conv, kcpCallback);
            kcp.NoDelay(1, 10, 2, 1);
            kcp.WndSize(64, 64);
            kcp.SetMtu(512);

            kcpCallback.OnReceive += OnReceive;
            kcpCallback.OnOutput += OnOutput;

            socket = new Socket(endPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            socket.Connect(endPoint);
        }

        private void OnReceive(byte[] buffer)
        {
            int count = Math.Min(asyncReceive.Size, buffer.Length);
            for (int i = 0; i < count; i++)
            {
                asyncReceive.Buffer.SetValue(asyncReceive.Offset + i, buffer[i]);
            }
            asyncReceive.EndReceive(count);
            Send(buffer);
        }

        private void OnOutput(Memory<byte> buffer)
        {
            if (socket != null)
            {
                socket.Send(buffer.ToArray());
            }
        }

        public void Update()
        {
            if (socket == null)
            {
                return;
            }

            kcp.Update(DateTime.UtcNow);

            int len;
            do
            {
                try
                {
                    var data = new byte[Message.MaxSize];
                    int size = socket.Receive(data, 0, Message.MaxSize, SocketFlags.None);
                    if (size > 0)
                    {
                        kcp.Input(data);
                    }
                }
                catch (Exception e)
                {
                    ConsoleUtility.WriteLine(e.Message);
                }

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

        public override void Send(byte[] buffer)
        {
            kcp.Send(buffer);
        }

        public override void Receive()
        {
            Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        Update();
                        await Task.Delay(5);
                    }
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
        }
    }
}
