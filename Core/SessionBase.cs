namespace Common
{
    public abstract class SessionBase : ISession
    {
        public abstract bool IsConnected { get; }
        public abstract void Send(byte[] buffer);
        public abstract void Receive();
        public abstract void Close();
    }
}
