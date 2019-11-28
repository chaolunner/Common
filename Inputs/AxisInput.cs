using MessagePack;

namespace Common
{
    [MessagePackObject]
    public class AxisInput : IInput
    {
        [Key(0)]
        public Fix64 Horizontal;
        [Key(1)]
        public Fix64 Vertical;
    }
}
