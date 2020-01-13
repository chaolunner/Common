using MessagePack;

namespace Common
{
    [MessagePackObject]
    public class EventInput : IInput
    {
        [Key(0)]
        public EventCode Type;
        [Key(1)]
        public string Message;

        public EventInput(EventCode type, string msg)
        {
            Type = type;
            Message = msg;
        }
    }
}
