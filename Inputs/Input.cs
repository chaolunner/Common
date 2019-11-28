using MessagePack;

namespace Common
{
    [Union(0, typeof(AxisInput))]
    public interface IInput { }
}
