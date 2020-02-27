using System.Collections.Generic;
using MessagePack;

namespace Common
{
    [MessagePackObject]
    public class EventInput : IInput
    {
        [Key(0)]
        public int Index;
        [Key(1)]
        public List<byte[]> Data;

        [IgnoreMember]
        public EventCode Type
        {
            get
            {
                return (EventCode)Index;
            }
        }

        public EventInput()
        {
            Data = new List<byte[]>();
        }

        public EventInput WithType(EventCode type)
        {
            Index = (int)type;
            return this;
        }

        public EventInput Add<T>(T obj, bool lz4 = true)
        {
            Data.Add(MessagePackUtility.Serialize(obj, lz4));
            return this;
        }

        public T Get<T>(int index, bool lz4 = true)
        {
            return MessagePackUtility.Deserialize<T>(Data[index], lz4);
        }
    }
}
