using MessagePack;

namespace Common
{
    [Union(0, typeof(EventInput))]
    [Union(1, typeof(AxisInput))]
    [Union(2, typeof(KeyInput))]
    [Union(3, typeof(MouseInput))]
    public interface IInput { }
}
