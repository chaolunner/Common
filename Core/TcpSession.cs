using System.Net.Sockets;
using System;

namespace Common
{
    public class TcpSession : SessionBase
    {
        private Socket socket;
        private IAsyncReceive asyncReceive;

        public TcpSession(Socket socket, IAsyncReceive asyncReceive)
        {
            this.socket = socket;
            this.asyncReceive = asyncReceive;
            BeginReceive();
        }

        private void BeginReceive()
        {
            if (IsConnected)
            {
                socket.BeginReceive(asyncReceive.Buffer, asyncReceive.Offset, asyncReceive.Size, SocketFlags.None, ReceiveCallback, null);
            }
            else
            {
                asyncReceive.EndReceive(0);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            if (IsConnected)
            {
                int count = socket.EndReceive(ar);
                asyncReceive.EndReceive(count);
                BeginReceive();
            }
            else
            {
                asyncReceive.EndReceive(0);
            }
        }

        public override bool IsConnected
        {
            get { return socket != null && socket.Connected; }
        }

        public override void Send(byte[] buffer)
        {
            if (IsConnected)
            {
                socket.Send(buffer);
            }
        }

        public override void Receive(IAsyncReceive asyncReceive, int count) { }

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
