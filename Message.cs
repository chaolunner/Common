using System;
using System.Linq;
using System.Text;

namespace Common
{
    public class Message
    {
        private byte[] data = new byte[MaxLength];
        private int startIndex = 0;
        private const int MaxLength = 8 * 1024;
        private const string MessagePackageTooLargeError = "Message package too large error! [{0}KB/{1}KB]";

        public byte[] Data
        {
            get { return data; }
        }

        public int StartIndex
        {
            get { return startIndex; }
        }

        public int RemainSize
        {
            get { return data.Length - startIndex; }
        }

        public void Process(int count, Action<RequestCode, byte[]> callback)
        {
            startIndex += count;
            while (true)
            {
                if (startIndex <= 4) { break; }
                int msgCount = BitConverter.ToInt32(data, 0);
                int totalCount = msgCount + 4;
                if (startIndex >= totalCount)
                {
                    RequestCode requestCode = (RequestCode)BitConverter.ToInt32(data, 4);
                    byte[] dataBytes = new byte[msgCount - 4];
                    Array.Copy(data, 8, dataBytes, 0, dataBytes.Length);
                    callback?.Invoke(requestCode, dataBytes);
                    Array.Copy(data, totalCount, data, 0, startIndex - totalCount);
                    startIndex -= totalCount;
                }
                else
                {
                    break;
                }
            }
        }

        public static byte[] Pack(RequestCode requestCode, byte[] dataBytes)
        {
            byte[] requestCodeBytes = BitConverter.GetBytes((int)requestCode);
            int length = requestCodeBytes.Length + dataBytes.Length;
            byte[] lengthBytes = BitConverter.GetBytes(length);
            int totalLength = length + lengthBytes.Length;
            if (totalLength > MaxLength)
            {
                ConsoleUtility.WriteLine(string.Format(MessagePackageTooLargeError, (totalLength / 1024f).ToString("F2"), (MaxLength / 1024f).ToString("F2")), ConsoleColor.Red);
            }
            return lengthBytes.Concat(requestCodeBytes).Concat(dataBytes).ToArray();
        }

        public static byte[] Pack(RequestCode requestCode, string data)
        {
            return Pack(requestCode, Encoding.UTF8.GetBytes(data));
        }
    }
}
