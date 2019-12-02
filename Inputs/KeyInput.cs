using System.Collections.Generic;
using UnityEngine;
using MessagePack;

namespace Common
{
    [MessagePackObject]
    public class KeyInput : IInput
    {
        [Key(0)]
        public List<KeyCode> KeyCodes;
    }
}
