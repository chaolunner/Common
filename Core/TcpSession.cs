using System.Net.Sockets;
using System;

namespace Common
{
    public class TcpSession : SessionBase
    {
        private Socket socket;
        private AsyncReceive asyncReceive;

        public TcpSession(Socket socket, AsyncReceive asyncReceive)
        {
            this.socket = socket;
            this.asyncReceive = asyncReceive;
        }

        private void BeginReceive()
        {
            if (socket != null && socket.Connected)
            {
                socket.BeginReceive(asyncReceive.Buffer, asyncReceive.Offset, asyncReceive.Size, SocketFlags.None, ReceiveCallback, null);
            }
            else
            {
                asyncReceive.EndReceive(0);
            }
        }

        public override void Send(byte[] buffer)
        {
            if (socket != null && socket.Connected)
            {
                socket.Send(buffer);
            }
        }

        public override void Receive()
        {
            BeginReceive();
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                int count = socket.EndReceive(ar);
                asyncReceive.EndReceive(count);
                BeginReceive();
            }
            catch (Exception e)
            {
                ConsoleUtility.WriteLine(e, ConsoleColor.Red);
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
        }
    }
}
