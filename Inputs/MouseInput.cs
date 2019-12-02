using System.Collections.Generic;
using MessagePack;

namespace Common
{
    [MessagePackObject]
    public class MouseInput : IInput
    {
        [Key(0)]
        public List<int> MouseButtons;
        [Key(1)]
        public FixVector2 ScrollDelta;
        [Key(2)]
        public FixVector2 Delta;
        [Key(3)]
        public FixVector3 Position;
    }
}
