﻿using MessagePack;
using System;

namespace Common
{
    [MessagePackObject]
    public struct FixVector2 : IEquatable<FixVector2>
    {
        #region Private Fields

        private static FixVector2 zeroVector = new FixVector2(0, 0);
        private static FixVector2 oneVector = new FixVector2(1, 1);

        private static FixVector2 rightVector = new FixVector2(1, 0);
        private static FixVector2 leftVector = new FixVector2(-1, 0);

        private static FixVector2 upVector = new FixVector2(0, 1);
        private static FixVector2 downVector = new FixVector2(0, -1);

        #endregion Private Fields

        #region Public Fields

        [Key(0)]
        public Fix64 x;
        [Key(1)]
        public Fix64 y;

        #endregion Public Fields

        #region Properties

        public static FixVector2 zero
        {
            get { return zeroVector; }
        }

        public static FixVector2 one
        {
            get { return oneVector; }
        }

        public static FixVector2 right
        {
            get { return rightVector; }
        }

        public static FixVector2 left
        {
            get { return leftVector; }
        }

        public static FixVector2 up
        {
            get { return upVector; }
        }

        public static FixVector2 down
        {
            get { return downVector; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Constructor foe standard 2D vector.
        /// </summary>
        /// <param name="x">
        /// A <see cref="System.Single"/>
        /// </param>
        /// <param name="y">
        /// A <see cref="System.Single"/>
        /// </param>
        public FixVector2(Fix64 x, Fix64 y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Constructor for "square" vector.
        /// </summary>
        /// <param name="value">
        /// A <see cref="System.Single"/>
        /// </param>
        public FixVector2(Fix64 value)
        {
            x = value;
            y = value;
        }

        public void Set(Fix64 x, Fix64 y)
        {
            this.x = x;
            this.y = y;
        }

        #endregion Constructors

        #region Public Methods

        public static void Reflect(ref FixVector2 vector, ref FixVector2 normal, out FixVector2 result)
        {
            Fix64 dot = Dot(vector, normal);
            result.x = vector.x - ((2f * dot) * normal.x);
            result.y = vector.y - ((2f * dot) * normal.y);
        }

        public static FixVector2 Reflect(FixVector2 vector, FixVector2 normal)
        {
            FixVector2 result;
            Reflect(ref vector, ref normal, out result);
            return result;
        }

        public static FixVector2 Add(FixVector2 value1, FixVector2 value2)
        {
            value1.x += value2.x;
            value1.y += value2.y;
            return value1;
        }

        public static void Add(ref FixVector2 value1, ref FixVector2 value2, out FixVector2 result)
        {
            result.x = value1.x + value2.x;
            result.y = value1.y + value2.y;
        }

        public static FixVector2 Barycentric(FixVector2 value1, FixVector2 value2, FixVector2 value3, Fix64 amount1, Fix64 amount2)
        {
            return new FixVector2(
                FixMath.Barycentric(value1.x, value2.x, value3.x, amount1, amount2),
                FixMath.Barycentric(value1.y, value2.y, value3.y, amount1, amount2));
        }

        public static void Barycentric(ref FixVector2 value1, ref FixVector2 value2, ref FixVector2 value3, Fix64 amount1,
                                       Fix64 amount2, out FixVector2 result)
        {
            result = new FixVector2(
                FixMath.Barycentric(value1.x, value2.x, value3.x, amount1, amount2),
                FixMath.Barycentric(value1.y, value2.y, value3.y, amount1, amount2));
        }

        public static FixVector2 CatmullRom(FixVector2 value1, FixVector2 value2, FixVector2 value3, FixVector2 value4, Fix64 amount)
        {
            return new FixVector2(
                FixMath.CatmullRom(value1.x, value2.x, value3.x, value4.x, amount),
                FixMath.CatmullRom(value1.y, value2.y, value3.y, value4.y, amount));
        }

        public static void CatmullRom(ref FixVector2 value1, ref FixVector2 value2, ref FixVector2 value3, ref FixVector2 value4,
                                      Fix64 amount, out FixVector2 result)
        {
            result = new FixVector2(
                FixMath.CatmullRom(value1.x, value2.x, value3.x, value4.x, amount),
                FixMath.CatmullRom(value1.y, value2.y, value3.y, value4.y, amount));
        }

        public static FixVector2 Clamp(FixVector2 value1, FixVector2 min, FixVector2 max)
        {
            return new FixVector2(
                FixMath.Clamp(value1.x, min.x, max.x),
                FixMath.Clamp(value1.y, min.y, max.y));
        }

        public static void Clamp(ref FixVector2 value1, ref FixVector2 min, ref FixVector2 max, out FixVector2 result)
        {
            result = new FixVector2(
                FixMath.Clamp(value1.x, min.x, max.x),
                FixMath.Clamp(value1.y, min.y, max.y));
        }

        /// <summary>
        /// Returns Fix64 precison distanve between two vectors
        /// </summary>
        /// <param name="value1">
        /// A <see cref="FixVector2"/>
        /// </param>
        /// <param name="value2">
        /// A <see cref="FixVector2"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Single"/>
        /// </returns>
        public static Fix64 Distance(FixVector2 value1, FixVector2 value2)
        {
            Fix64 result;
            DistanceSquared(ref value1, ref value2, out result);
            return (Fix64)Fix64.Sqrt(result);
        }


        public static void Distance(ref FixVector2 value1, ref FixVector2 value2, out Fix64 result)
        {
            DistanceSquared(ref value1, ref value2, out result);
            result = (Fix64)Fix64.Sqrt(result);
        }

        public static Fix64 DistanceSquared(FixVector2 value1, FixVector2 value2)
        {
            Fix64 result;
            DistanceSquared(ref value1, ref value2, out result);
            return result;
        }

        public static void DistanceSquared(ref FixVector2 value1, ref FixVector2 value2, out Fix64 result)
        {
            result = (value1.x - value2.x) * (value1.x - value2.x) + (value1.y - value2.y) * (value1.y - value2.y);
        }

        public static void Divide(ref FixVector2 value1, ref FixVector2 value2, out FixVector2 result)
        {
            result.x = value1.x / value2.x;
            result.y = value1.y / value2.y;
        }

        public static FixVector2 Divide(FixVector2 value1, Fix64 scaleFactor)
        {
            FixVector2 result;
            FixVector2.Divide(ref value1, scaleFactor, out result);
            return result;
        }

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <param name="result">Returns the scaled vector.</param>
        public static void Divide(ref FixVector2 value1, Fix64 scaleFactor, out FixVector2 result)
        {
            result.x = value1.x / scaleFactor;
            result.y = value1.y / scaleFactor;
        }

        public static Fix64 Dot(FixVector2 value1, FixVector2 value2)
        {
            return value1.x * value2.x + value1.y * value2.y;
        }

        public static void Dot(ref FixVector2 value1, ref FixVector2 value2, out Fix64 result)
        {
            result = value1.x * value2.x + value1.y * value2.y;
        }

        public override bool Equals(object obj)
        {
            return (obj is FixVector2) ? this == ((FixVector2)obj) : false;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode();
        }

        public bool Equals(FixVector2 other)
        {
            return this == other;
        }

        public static FixVector2 Hermite(FixVector2 value1, FixVector2 tangent1, FixVector2 value2, FixVector2 tangent2, Fix64 amount)
        {
            FixVector2 result = new FixVector2();
            Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
            return result;
        }

        public static void Hermite(ref FixVector2 value1, ref FixVector2 tangent1, ref FixVector2 value2, ref FixVector2 tangent2,
                                   Fix64 amount, out FixVector2 result)
        {
            result.x = FixMath.Hermite(value1.x, tangent1.x, value2.x, tangent2.x, amount);
            result.y = FixMath.Hermite(value1.y, tangent1.y, value2.y, tangent2.y, amount);
        }

        [IgnoreMember]
        public Fix64 magnitude
        {
            get
            {
                Fix64 result;
                DistanceSquared(ref this, ref zeroVector, out result);
                return Fix64.Sqrt(result);
            }
        }

        public static FixVector2 ClampMagnitude(FixVector2 vector, Fix64 maxLength)
        {
            return Normalize(vector) * maxLength;
        }

        public Fix64 LengthSquared()
        {
            Fix64 result;
            DistanceSquared(ref this, ref zeroVector, out result);
            return result;
        }

        public static FixVector2 Lerp(FixVector2 value1, FixVector2 value2, Fix64 amount)
        {
            amount = FixMath.Clamp(amount, 0, 1);

            return new FixVector2(
                FixMath.Lerp(value1.x, value2.x, amount),
                FixMath.Lerp(value1.y, value2.y, amount));
        }

        public static FixVector2 LerpUnclamped(FixVector2 value1, FixVector2 value2, Fix64 amount)
        {
            return new FixVector2(
                FixMath.Lerp(value1.x, value2.x, amount),
                FixMath.Lerp(value1.y, value2.y, amount));
        }

        public static void LerpUnclamped(ref FixVector2 value1, ref FixVector2 value2, Fix64 amount, out FixVector2 result)
        {
            result = new FixVector2(
                FixMath.Lerp(value1.x, value2.x, amount),
                FixMath.Lerp(value1.y, value2.y, amount));
        }

        public static FixVector2 Max(FixVector2 value1, FixVector2 value2)
        {
            return new FixVector2(
                FixMath.Max(value1.x, value2.x),
                FixMath.Max(value1.y, value2.y));
        }

        public static void Max(ref FixVector2 value1, ref FixVector2 value2, out FixVector2 result)
        {
            result.x = FixMath.Max(value1.x, value2.x);
            result.y = FixMath.Max(value1.y, value2.y);
        }

        public static FixVector2 Min(FixVector2 value1, FixVector2 value2)
        {
            return new FixVector2(
                FixMath.Min(value1.x, value2.x),
                FixMath.Min(value1.y, value2.y));
        }

        public static void Min(ref FixVector2 value1, ref FixVector2 value2, out FixVector2 result)
        {
            result.x = FixMath.Min(value1.x, value2.x);
            result.y = FixMath.Min(value1.y, value2.y);
        }

        public void Scale(FixVector2 other)
        {
            this.x = x * other.x;
            this.y = y * other.y;
        }

        public static FixVector2 Scale(FixVector2 value1, FixVector2 value2)
        {
            FixVector2 result;
            result.x = value1.x * value2.x;
            result.y = value1.y * value2.y;

            return result;
        }

        public static FixVector2 Multiply(FixVector2 value1, FixVector2 value2)
        {
            value1.x *= value2.x;
            value1.y *= value2.y;
            return value1;
        }

        public static FixVector2 Multiply(FixVector2 value1, Fix64 scaleFactor)
        {
            value1.x *= scaleFactor;
            value1.y *= scaleFactor;
            return value1;
        }

        public static void Multiply(ref FixVector2 value1, Fix64 scaleFactor, out FixVector2 result)
        {
            result.x = value1.x * scaleFactor;
            result.y = value1.y * scaleFactor;
        }

        public static void Multiply(ref FixVector2 value1, ref FixVector2 value2, out FixVector2 result)
        {
            result.x = value1.x * value2.x;
            result.y = value1.y * value2.y;
        }

        public static FixVector2 Negate(FixVector2 value)
        {
            value.x = -value.x;
            value.y = -value.y;
            return value;
        }

        public static void Negate(ref FixVector2 value, out FixVector2 result)
        {
            result.x = -value.x;
            result.y = -value.y;
        }

        public void Normalize()
        {
            Normalize(ref this, out this);
        }

        public static FixVector2 Normalize(FixVector2 value)
        {
            Normalize(ref value, out value);
            return value;
        }

        [IgnoreMember]
        public FixVector2 normalized
        {
            get
            {
                FixVector2 result;
                FixVector2.Normalize(ref this, out result);

                return result;
            }
        }

        public static void Normalize(ref FixVector2 value, out FixVector2 result)
        {
            Fix64 factor;
            DistanceSquared(ref value, ref zeroVector, out factor);
            factor = 1f / (Fix64)Fix64.Sqrt(factor);
            result.x = value.x * factor;
            result.y = value.y * factor;
        }

        public static FixVector2 SmoothStep(FixVector2 value1, FixVector2 value2, Fix64 amount)
        {
            return new FixVector2(
                FixMath.SmoothStep(value1.x, value2.x, amount),
                FixMath.SmoothStep(value1.y, value2.y, amount));
        }

        public static void SmoothStep(ref FixVector2 value1, ref FixVector2 value2, Fix64 amount, out FixVector2 result)
        {
            result = new FixVector2(
                FixMath.SmoothStep(value1.x, value2.x, amount),
                FixMath.SmoothStep(value1.y, value2.y, amount));
        }

        public static FixVector2 Subtract(FixVector2 value1, FixVector2 value2)
        {
            value1.x -= value2.x;
            value1.y -= value2.y;
            return value1;
        }

        public static void Subtract(ref FixVector2 value1, ref FixVector2 value2, out FixVector2 result)
        {
            result.x = value1.x - value2.x;
            result.y = value1.y - value2.y;
        }

        public static Fix64 Angle(FixVector2 a, FixVector2 b)
        {
            return Fix64.Acos(a.normalized * b.normalized) * Fix64.Rad2Deg;
        }

        public FixVector3 ToVector3()
        {
            return new FixVector3(this.x, this.y, 0);
        }

        public override string ToString()
        {
            return string.Format("({0:f1}, {1:f1})", x.AsFloat(), y.AsFloat());
        }

        #endregion Public Methods

        #region Operators

        public static FixVector2 operator -(FixVector2 value)
        {
            value.x = -value.x;
            value.y = -value.y;
            return value;
        }


        public static bool operator ==(FixVector2 value1, FixVector2 value2)
        {
            return value1.x == value2.x && value1.y == value2.y;
        }


        public static bool operator !=(FixVector2 value1, FixVector2 value2)
        {
            return value1.x != value2.x || value1.y != value2.y;
        }


        public static FixVector2 operator +(FixVector2 value1, FixVector2 value2)
        {
            FixVector2 result;
            FixVector2.Add(ref value1, ref value2, out result);
            return result;
        }


        public static FixVector2 operator -(FixVector2 value1, FixVector2 value2)
        {
            FixVector2 result;
            FixVector2.Subtract(ref value1, ref value2, out result);
            return result;
        }


        public static Fix64 operator *(FixVector2 value1, FixVector2 value2)
        {
            return FixVector2.Dot(value1, value2);
        }


        public static FixVector2 operator *(FixVector2 value, Fix64 scaleFactor)
        {
            FixVector2 result;
            FixVector2.Multiply(ref value, scaleFactor, out result);
            return result;
        }


        public static FixVector2 operator *(Fix64 scaleFactor, FixVector2 value)
        {
            FixVector2 result;
            FixVector2.Multiply(ref value, scaleFactor, out result);
            return result;
        }


        public static FixVector2 operator /(FixVector2 value1, Fix64 scaleFactor)
        {
            FixVector2 result;
            FixVector2.Divide(ref value1, scaleFactor, out result);
            return result;
        }

#if UNITY_5_3_OR_NEWER || UNITY_2017_1_OR_NEWER
        public static explicit operator FixVector2(UnityEngine.Vector2 vector2)
        {
            return new FixVector2((Fix64)vector2.x, (Fix64)vector2.y);
        }

        public static explicit operator UnityEngine.Vector2(FixVector2 vector2)
        {
            return new UnityEngine.Vector2((float)vector2.x, (float)vector2.y);
        }
#endif

        #endregion Operators
    }
}
