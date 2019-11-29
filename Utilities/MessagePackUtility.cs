using MessagePack;

namespace Common
{
    public static class MessagePackUtility
    {
        public static MessagePackSerializerOptions LZ4Standard = MessagePackSerializer.DefaultOptions.WithLZ4Compression(true);

        public static byte[] Serialize<T>(T obj, bool lz4 = true)
        {
            if (lz4)
            {
                return MessagePackSerializer.Serialize(obj, LZ4Standard);
            }
            return MessagePackSerializer.Serialize(obj);
        }

        public static T Deserialize<T>(byte[] bytes, bool lz4 = true)
        {
            if (lz4)
            {
                return MessagePackSerializer.Deserialize<T>(bytes, LZ4Standard);
            }
            return MessagePackSerializer.Deserialize<T>(bytes);
        }

        public static byte[] FromJson(string str, bool lz4 = true)
        {
            if (lz4)
            {
                return MessagePackSerializer.ConvertFromJson(str, LZ4Standard);
            }
            return MessagePackSerializer.ConvertFromJson(str);
        }

        public static string ToJson(byte[] bytes, bool lz4 = true)
        {
            if (lz4)
            {
                return MessagePackSerializer.ConvertToJson(bytes, LZ4Standard);
            }
            return MessagePackSerializer.ConvertToJson(bytes);
        }
    }
}
