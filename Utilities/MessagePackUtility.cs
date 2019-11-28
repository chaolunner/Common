using MessagePack;

namespace Common
{
    public static class MessagePackUtility
    {
        public static byte[] Serialize<T>(T obj, bool lz4 = true)
        {
            if (lz4)
            {
                return MessagePackSerializer.Serialize(obj, MessagePackSerializerOptions.LZ4Standard);
            }
            return MessagePackSerializer.Serialize(obj, MessagePackSerializerOptions.Standard);
        }

        public static T Deserialize<T>(byte[] bytes, bool lz4 = true)
        {
            if (lz4)
            {
                return MessagePackSerializer.Deserialize<T>(bytes, MessagePackSerializerOptions.LZ4Standard);
            }
            return MessagePackSerializer.Deserialize<T>(bytes, MessagePackSerializerOptions.Standard);
        }

        public static byte[] FromJson(string str, bool lz4 = true)
        {
            if (lz4)
            {
                return MessagePackSerializer.ConvertFromJson(str, MessagePackSerializerOptions.LZ4Standard);
            }
            return MessagePackSerializer.ConvertFromJson(str, MessagePackSerializerOptions.Standard);
        }

        public static string ToJson(byte[] bytes, bool lz4 = true)
        {
            if (lz4)
            {
                return MessagePackSerializer.ConvertToJson(bytes, MessagePackSerializerOptions.LZ4Standard);
            }
            return MessagePackSerializer.ConvertToJson(bytes, MessagePackSerializerOptions.Standard);
        }
    }
}
