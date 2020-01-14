using MessagePack;

namespace Common
{
    [MessagePackObject]
    public struct LockstepInputs
    {
        [Key(0)]
        public int TickId;
        [Key(1)]
        public Fix64 DeltaTime;
        [Key(2)]
        public UserInputs[][] UserInputs;
    }
}
