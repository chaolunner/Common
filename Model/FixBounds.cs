using MessagePack;
using System;

namespace Common
{
    [MessagePackObject]
    public struct FixBounds : IEquatable<FixBounds>
    {
        [Key(0)]
        public FixVector3 Position;
        [Key(1)]
        public FixVector3 Size;

        [IgnoreMember]
        public Fix64 x
        {
            get { return Position.x; }
            set { Position.x = value; }
        }

        [IgnoreMember]
        public Fix64 y
        {
            get { return Position.y; }
            set { Position.y = value; }
        }

        [IgnoreMember]
        public Fix64 z
        {
            get { return Position.z; }
            set { Position.z = value; }
        }

        [IgnoreMember]
        public FixVector3 center
        {
            get { return new FixVector3(x + Size.x / 2f, y + Size.y / 2f, z + Size.z / 2f); }
        }

        [IgnoreMember]
        public FixVector3 min
        {
            get { return new FixVector3(xMin, yMin, zMin); }
            set
            {
                xMin = value.x;
                yMin = value.y;
                zMin = value.z;
            }
        }

        [IgnoreMember]
        public FixVector3 max
        {
            get { return new FixVector3(xMax, yMax, zMax); }
            set
            {
                xMax = value.x;
                yMax = value.y;
                zMax = value.z;
            }
        }

        [IgnoreMember]
        public Fix64 xMin
        {
            get { return FixMath.Min(Position.x, Position.x + Size.x); }
            set
            {
                Fix64 oldxmax = xMax;
                Position.x = value;
                Size.x = oldxmax - Position.x;
            }
        }

        [IgnoreMember]
        public Fix64 yMin
        {
            get { return FixMath.Min(Position.y, Position.y + Size.y); }
            set
            {
                Fix64 oldymax = yMax;
                Position.y = value;
                Size.y = oldymax - Position.y;
            }
        }

        [IgnoreMember]
        public Fix64 zMin
        {
            get { return FixMath.Min(Position.z, Position.z + Size.z); }
            set
            {
                Fix64 oldzmax = zMax;
                Position.z = value;
                Size.z = oldzmax - Position.z;
            }
        }

        [IgnoreMember]
        public Fix64 xMax
        {
            get { return FixMath.Max(Position.x, Position.x + Size.x); }
            set { Size.x = value - Position.x; }
        }

        [IgnoreMember]
        public Fix64 yMax
        {
            get { return FixMath.Max(Position.y, Position.y + Size.y); }
            set { Size.y = value - Position.y; }
        }

        [IgnoreMember]
        public Fix64 zMax
        {
            get { return FixMath.Max(Position.z, Position.z + Size.z); }
            set { Size.z = value - Position.z; }
        }

        public FixBounds(Fix64 xMin, Fix64 yMin, Fix64 zMin, Fix64 sizeX, Fix64 sizeY, Fix64 sizeZ)
        {
            Position = new FixVector3(xMin, yMin, zMin);
            Size = new FixVector3(sizeX, sizeY, sizeZ);
        }

        public FixBounds(FixVector3 position, FixVector3 size)
        {
            Position = position;
            this.Size = size;
        }

        public void SetMinMax(FixVector3 minPosition, FixVector3 maxPosition)
        {
            min = minPosition;
            max = maxPosition;
        }

        public void ClampToBounds(FixBounds bounds)
        {
            Position = new FixVector3(
                FixMath.Max(FixMath.Min(bounds.xMax, Position.x), bounds.xMin),
                FixMath.Max(FixMath.Min(bounds.yMax, Position.y), bounds.yMin),
                FixMath.Max(FixMath.Min(bounds.zMax, Position.z), bounds.zMin)
            );
            Size = new FixVector3(
                FixMath.Min(bounds.xMax - Position.x, Size.x),
                FixMath.Min(bounds.yMax - Position.y, Size.y),
                FixMath.Min(bounds.zMax - Position.z, Size.z)
            );
        }

        public bool Contains(FixVector3 position)
        {
            return position.x >= xMin
                   && position.y >= yMin
                   && position.z >= zMin
                   && position.x < xMax
                   && position.y < yMax
                   && position.z < zMax;
        }

        public override string ToString()
        {
            return string.Format("Position: {0}, Size: {1}", Position, Size);
        }

        public static bool operator ==(FixBounds lhs, FixBounds rhs)
        {
            return lhs.Position == rhs.Position && lhs.Size == rhs.Size;
        }

        public static bool operator !=(FixBounds lhs, FixBounds rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object other)
        {
            return other is FixBounds && (FixBounds)other == this;
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode() ^ (Size.GetHashCode() << 2);
        }

        public bool Equals(FixBounds other)
        {
            return this == other;
        }
    }
}
