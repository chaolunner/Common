using MessagePack;

namespace Common
{
    [MessagePackObject]
    public struct UserInputs
    {
        [Key(0)]
        public int Number;
        [Key(1)]
        public int TickId;
        [Key(2)]
        public int UserId;
        [Key(3)]
        public byte[][] InputData;
    }
}
