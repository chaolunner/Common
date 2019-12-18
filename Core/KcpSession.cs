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
        private Socket tcpSocket;
        private Socket udpSocket;
        private AsyncReceive tcpAsyncReceive;
        private AsyncReceive udpAsyncReceive;
        private const int conv = 0;
        private const string KcpSendError = "kcp send error";

        public KcpSession(Socket socket, AsyncReceive asyncReceive)
        {
            tcpSocket = socket;
            tcpAsyncReceive = asyncReceive;
            Connect(socket.RemoteEndPoint);
            udpAsyncReceive = new AsyncReceive(new Message(), ReceiveCallback);
        }

        private void Connect(EndPoint endPoint)
        {
            kcpCallback = new KcpCallback();
            kcp = new Kcp(conv, kcpCallback);
            kcp.NoDelay(1, 10, 2, 1);
            kcp.WndSize(64, 64);
            kcp.SetMtu(512);

            kcpCallback.OnReceive += OnReceive;
            kcpCallback.OnOutput += OnOutput;

            udpSocket = new Socket(endPoint.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            udpSocket.Connect(endPoint);
        }

        private void OnReceive(byte[] buffer)
        {
            int count = Math.Min(tcpAsyncReceive.Size, buffer.Length);
            for (int i = 0; i < count; i++)
            {
                tcpAsyncReceive.Buffer.SetValue(tcpAsyncReceive.Offset + i, buffer[i]);
            }
            tcpAsyncReceive.EndReceive(count);
            Send(buffer);
        }

        private void OnOutput(Memory<byte> buffer)
        {
            if (IsConnected)
            {
                udpSocket.Send(buffer.ToArray());
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
                udpSocket.BeginReceive(udpAsyncReceive.Buffer, udpAsyncReceive.Offset, udpAsyncReceive.Size, SocketFlags.None, ReceiveCallback, null);
            }
            else
            {
                udpAsyncReceive.EndReceive(0);
            }
        }

        private void ReceiveCallback(int count)
        {
            if (count > 0)
            {
                kcp.Input(udpAsyncReceive.Buffer);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            if (IsConnected)
            {
                int count = udpSocket.EndReceive(ar);
                udpAsyncReceive.EndReceive(count);
                BeginReceive();
            }
            else
            {
                udpAsyncReceive.EndReceive(0);
            }
        }

        public override bool IsConnected
        {
            get { return tcpSocket != null && udpSocket != null && kcp != null && tcpSocket.Connected && udpSocket.Connected; }
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
            if (tcpSocket != null)
            {
                tcpSocket.Close();
                tcpSocket = null;
            }
            if (udpSocket != null)
            {
                udpSocket.Close();
                udpSocket = null;
            }
            if (kcp != null)
            {
                kcp.Dispose();
                kcp = null;
            }
        }
    }
}
