using MessagePack;

namespace Common
{
    [Union(0, typeof(AxisInput))]
    [Union(1, typeof(KeyInput))]
    [Union(2, typeof(MouseInput))]
    public interface IInput { }
}
