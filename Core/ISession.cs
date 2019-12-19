namespace Common
{
    public interface ISession
    {
        bool IsConnected { get; }
        void Send(byte[] buffer);
        void Receive(IAsyncReceive asyncReceive, int count);
        void Close();
    }
}
