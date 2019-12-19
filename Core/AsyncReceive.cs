using System;

namespace Common
{
    public class AsyncReceive : IAsyncReceive
    {
        private byte[] buffer;
        private int offset;
        private int size;
        private Action<int> asyncCallback;

        public byte[] Buffer { get { return buffer; } }

        public int Offset { get { return offset; } }

        public int Size { get { return size; } }

        public AsyncReceive(int size = Message.MaxSize)
        {
            this.size = size;
            buffer = new byte[size];
        }

        public void BeginReceive(Action<int> callback)
        {
            asyncCallback = callback;
        }

        public void EndReceive(int count)
        {
            asyncCallback?.Invoke(count);
        }
    }
}
