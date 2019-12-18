using System.Net.Sockets;
using System.Net;

namespace Common
{
    public interface ISession
    {
        bool IsConnected { get; }
        Socket Bind(IPEndPoint ipEndPoint);
        void Send(byte[] buffer);
        void Receive();
        void Close();
    }
}
