using System.Collections.Generic;
using MessagePack;

namespace Common
{
    [MessagePackObject]
    public class EventInput : IInput
    {
        private const char VerticalBar = '|';

        [Key(0)]
        public Dictionary<EventCode, string> Events;

        public EventInput()
        {
            Events = new Dictionary<EventCode, string>();
        }

        public void Write(EventCode type, string msg)
        {
            if (!Events.ContainsKey(type))
            {
                Events.Add(type, msg);
            }
            else
            {
                Events[type] += VerticalBar + msg;
            }
        }

        public string[] Read(EventCode type)
        {
            if (Events.ContainsKey(type))
            {
                return Events[type].Split(VerticalBar);
            }
            return null;
        }
    }
}
