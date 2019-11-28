using MessagePack;

namespace Common
{
    [MessagePackObject]
    public struct UserInputs
    {
        [Key(0)]
        public int UserId;
        [Key(1)]
        public byte[][] InputData;
    }
}
