using System.Collections.Generic;
using MessagePack;

namespace Common
{
    [MessagePackObject]
    public class EventInput : IInput
    {
        [Key(0)]
        public List<EventData> Events;
    }

    [MessagePackObject]
    public struct EventData
    {
        [Key(0)]
        public EventType Type;
        [Key(1)]
        public string Message;
    }
}
