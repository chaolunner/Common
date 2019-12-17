namespace Common
{
    public interface ISession
    {
        void Send(byte[] buffer);
        void Receive();
        void Close();
    }
}
