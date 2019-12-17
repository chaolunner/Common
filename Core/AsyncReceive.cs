using System;

namespace Common
{
    public class AsyncReceive
    {
        private Message message;
        private Action<int> asyncCallback;

        public byte[] Buffer
        {
            get
            {
                return message.Data;
            }
        }

        public int Offset
        {
            get
            {
                return message.StartIndex;
            }
        }

        public int Size
        {
            get
            {
                return message.RemainSize;
            }
        }

        public AsyncReceive(Message msg, Action<int> callback)
        {
            message = msg;
            asyncCallback = callback;
        }

        public void EndReceive(int count)
        {
            asyncCallback?.Invoke(count);
        }
    }
}
