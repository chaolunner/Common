using System.Text;

namespace Common
{
    public struct ReceiveData
    {
        public byte[] Value;
        public SessionMode Mode;

        public string StringValue
        {
            get
            {
                return Encoding.UTF8.GetString(Value);
            }
        }

        public ReceiveData(byte[] value, SessionMode mode)
        {
            Value = value;
            Mode = mode;
        }
    }
}
