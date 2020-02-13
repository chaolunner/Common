namespace Common
{
    public abstract class SessionBase : ISession
    {
        public abstract bool IsConnected { get; }
        public SessionMode Mode { get; set; } = SessionMode.Online;
        public abstract void Send(byte[] buffer);
        public abstract void Receive(IAsyncReceive asyncReceive, int count);
        public abstract void Close();
    }
}
