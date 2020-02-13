namespace Common
{
    public interface ISession
    {
        bool IsConnected { get; }
        SessionMode Mode { get; set; }
        void Send(byte[] buffer);
        void Receive(IAsyncReceive asyncReceive, int count);
        void Close();
    }
}
