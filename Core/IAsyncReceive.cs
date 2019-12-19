using System;

namespace Common
{
    public interface IAsyncReceive
    {
        byte[] Buffer { get; }
        int Offset { get; }
        int Size { get; }
        void BeginReceive(Action<int> callback);
        void EndReceive(int count);
    }
}
