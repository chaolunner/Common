using System.Collections.Generic;
using MessagePack;

namespace Common
{
    [MessagePackObject]
    public class KeyInput : IInput
    {
        [Key(0)]
        public List<int> KeyCodes;
    }
}
