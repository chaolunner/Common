using System;

namespace Common
{
    public class MessageAsyncReceive : IAsyncReceive
    {
        private Message message;
        private Action<int> asyncCallback;

        public byte[] Buffer { get { return message.Data; } }

        public int Offset { get { return message.StartIndex; } }

        public int Size { get { return message.RemainSize; } }

        public MessageAsyncReceive(Message msg)
        {
            message = msg;
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
