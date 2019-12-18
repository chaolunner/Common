using System.Net.Sockets;
using System.Net;

namespace Common
{
    public abstract class SessionBase : ISession
    {
        public abstract bool IsConnected { get; }
        public abstract Socket Bind(IPEndPoint ipEndPoint);
        public abstract void Send(byte[] buffer);
        public abstract void Receive();
        public abstract void Close();
    }
}
