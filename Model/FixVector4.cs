﻿using MessagePack;
using System;

namespace Common
{
    [MessagePackObject]
    public struct FixVector4 : IEquatable<FixVector4>
    {
        private static Fix64 ZeroEpsilonSq = FixMath.Epsilon;
        internal static FixVector4 InternalZero;

        [Key(0)]
        public Fix64 x;
        [Key(1)]
        public Fix64 y;
        [Key(2)]
        public Fix64 z;
        [Key(3)]
        public Fix64 w;

        #region Static readonly variables
        /// <summary>
        /// A vector with components (0,0,0,0);
        /// </summary>
        public static readonly FixVector4 zero;
        /// <summary>
        /// A vector with components (1,1,1,1);
        /// </summary>
        public static readonly FixVector4 one;
        /// <summary>
        /// A vector with components 
        /// (Fix64.MinValue,Fix64.MinValue,Fix64.MinValue);
        /// </summary>
        public static readonly FixVector4 MinValue;
        /// <summary>
        /// A vector with components 
        /// (Fix64.MaxValue,Fix64.MaxValue,Fix64.MaxValue);
        /// </summary>
        public static readonly FixVector4 MaxValue;
        #endregion

        #region Private static constructor
        static FixVector4()
        {
            one = new FixVector4(1, 1, 1, 1);
            zero = new FixVector4(0, 0, 0, 0);
            MinValue = new FixVector4(Fix64.MinValue);
            MaxValue = new FixVector4(Fix64.MaxValue);
            InternalZero = zero;
        }
        #endregion

        public static FixVector4 Abs(FixVector4 other)
        {
            return new FixVector4(Fix64.Abs(other.x), Fix64.Abs(other.y), Fix64.Abs(other.z), Fix64.Abs(other.z));
        }

        /// <summary>
        /// Gets the squared length of the vector.
        /// </summary>
        /// <returns>Returns the squared length of the vector.</returns>
        [IgnoreMember]
        public Fix64 sqrMagnitude
        {
            get
            {
                return (((this.x * this.x) + (this.y * this.y)) + (this.z * this.z) + (this.w * this.w));
            }
        }

        /// <summary>
        /// Gets the length of the vector.
        /// </summary>
        /// <returns>Returns the length of the vector.</returns>
        [IgnoreMember]
        public Fix64 magnitude
        {
            get
            {
                Fix64 num = sqrMagnitude;
                return Fix64.Sqrt(num);
            }
        }

        public static FixVector4 ClampMagnitude(FixVector4 vector, Fix64 maxLength)
        {
            return Normalize(vector) * maxLength;
        }

        /// <summary>
        /// Gets a normalized version of the vector.
        /// </summary>
        /// <returns>Returns a normalized version of the vector.</returns>
        [IgnoreMember]
        public FixVector4 normalized
        {
            get
            {
                FixVector4 result = new FixVector4(this.x, this.y, this.z, this.w);
                result.Normalize();

                return result;
            }
        }

        /// <summary>
        /// Constructor initializing a new instance of the structure
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>
        /// <param name="w">The W component of the vector.</param>
        public FixVector4(int x, int y, int z, int w)
        {
            this.x = (Fix64)x;
            this.y = (Fix64)y;
            this.z = (Fix64)z;
            this.w = (Fix64)w;
        }

        public FixVector4(Fix64 x, Fix64 y, Fix64 z, Fix64 w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// Multiplies each component of the vector by the same components of the provided vector.
        /// </summary>
        public void Scale(FixVector4 other)
        {
            this.x = x * other.x;
            this.y = y * other.y;
            this.z = z * other.z;
            this.w = w * other.w;
        }

        /// <summary>
        /// Sets all vector component to specific values.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>
        /// <param name="w">The W component of the vector.</param>
        public void Set(Fix64 x, Fix64 y, Fix64 z, Fix64 w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// Constructor initializing a new instance of the structure
        /// </summary>
        /// <param name="xyz">All components of the vector are set to xyz</param>
        public FixVector4(Fix64 xyzw)
        {
            this.x = xyzw;
            this.y = xyzw;
            this.z = xyzw;
            this.w = xyzw;
        }

        public static FixVector4 Lerp(FixVector4 from, FixVector4 to, Fix64 percent)
        {
            return from + (to - from) * percent;
        }

        /// <summary>
        /// Builds a string from the FixVector4.
        /// </summary>
        /// <returns>A string containing all three components.</returns>
        #region public override string ToString()
        public override string ToString()
        {
            return string.Format("({0:f1}, {1:f1}, {2:f1}, {3:f1})", x.AsFloat(), y.AsFloat(), z.AsFloat(), w.AsFloat());
        }
        #endregion

        /// <summary>
        /// Multiplies each component of the vector by the same components of the provided vector.
        /// </summary>
        public static FixVector4 Scale(FixVector4 vecA, FixVector4 vecB)
        {
            FixVector4 result;
            result.x = vecA.x * vecB.x;
            result.y = vecA.y * vecB.y;
            result.z = vecA.z * vecB.z;
            result.w = vecA.w * vecB.w;

            return result;
        }

        /// <summary>
        /// Tests if two FixVector4 are equal.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>Returns true if both values are equal, otherwise false.</returns>
        public static bool operator ==(FixVector4 value1, FixVector4 value2)
        {
            return (((value1.x == value2.x) && (value1.y == value2.y)) && (value1.z == value2.z) && (value1.w == value2.w));
        }

        /// <summary>
        /// Tests if two FixVector4 are not equal.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>Returns false if both values are equal, otherwise true.</returns>
        public static bool operator !=(FixVector4 value1, FixVector4 value2)
        {
            if ((value1.x == value2.x) && (value1.y == value2.y) && (value1.z == value2.z))
            {
                return (value1.w != value2.w);
            }
            return true;
        }

        /// <summary>
        /// Gets a vector with the minimum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>A vector with the minimum x,y and z values of both vectors.</returns>
        public static FixVector4 Min(FixVector4 value1, FixVector4 value2)
        {
            FixVector4 result;
            FixVector4.Min(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Gets a vector with the minimum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="result">A vector with the minimum x,y and z values of both vectors.</param>
        public static void Min(ref FixVector4 value1, ref FixVector4 value2, out FixVector4 result)
        {
            result.x = (value1.x < value2.x) ? value1.x : value2.x;
            result.y = (value1.y < value2.y) ? value1.y : value2.y;
            result.z = (value1.z < value2.z) ? value1.z : value2.z;
            result.w = (value1.w < value2.w) ? value1.w : value2.w;
        }

        /// <summary>
        /// Gets a vector with the maximum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>A vector with the maximum x,y and z values of both vectors.</returns>
        public static FixVector4 Max(FixVector4 value1, FixVector4 value2)
        {
            FixVector4 result;
            FixVector4.Max(ref value1, ref value2, out result);
            return result;
        }

        public static Fix64 Distance(FixVector4 v1, FixVector4 v2)
        {
            return Fix64.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y) + (v1.z - v2.z) * (v1.z - v2.z) + (v1.w - v2.w) * (v1.w - v2.w));
        }

        /// <summary>
        /// Gets a vector with the maximum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="result">A vector with the maximum x,y and z values of both vectors.</param>
        public static void Max(ref FixVector4 value1, ref FixVector4 value2, out FixVector4 result)
        {
            result.x = (value1.x > value2.x) ? value1.x : value2.x;
            result.y = (value1.y > value2.y) ? value1.y : value2.y;
            result.z = (value1.z > value2.z) ? value1.z : value2.z;
            result.w = (value1.w > value2.w) ? value1.w : value2.w;
        }

        /// <summary>
        /// Sets the length of the vector to zero.
        /// </summary>
        #region public void MakeZero()
        public void MakeZero()
        {
            x = Fix64.Zero;
            y = Fix64.Zero;
            z = Fix64.Zero;
            w = Fix64.Zero;
        }
        #endregion

        /// <summary>
        /// Checks if the length of the vector is zero.
        /// </summary>
        /// <returns>Returns true if the vector is zero, otherwise false.</returns>
        #region public bool IsZero()
        public bool IsZero()
        {
            return (this.sqrMagnitude == Fix64.Zero);
        }

        /// <summary>
        /// Checks if the length of the vector is nearly zero.
        /// </summary>
        /// <returns>Returns true if the vector is nearly zero, otherwise false.</returns>
        public bool IsNearlyZero()
        {
            return (this.sqrMagnitude < ZeroEpsilonSq);
        }
        #endregion

        /// <summary>
        /// Transforms a vector by the given matrix.
        /// </summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transform matrix.</param>
        /// <returns>The transformed vector.</returns>
        public static FixVector4 Transform(FixVector4 position, FixMatrix4x4 matrix)
        {
            FixVector4 result;
            FixVector4.Transform(ref position, ref matrix, out result);
            return result;
        }

        public static FixVector4 Transform(FixVector3 position, FixMatrix4x4 matrix)
        {
            FixVector4 result;
            FixVector4.Transform(ref position, ref matrix, out result);
            return result;
        }

        /// <summary>
        /// Transforms a vector by the given matrix.
        /// </summary>
        /// <param name="vector">The vector to transform.</param>
        /// <param name="matrix">The transform matrix.</param>
        /// <param name="result">The transformed vector.</param>
        public static void Transform(ref FixVector3 vector, ref FixMatrix4x4 matrix, out FixVector4 result)
        {
            result.x = vector.x * matrix.M11 + vector.y * matrix.M12 + vector.z * matrix.M13 + matrix.M14;
            result.y = vector.x * matrix.M21 + vector.y * matrix.M22 + vector.z * matrix.M23 + matrix.M24;
            result.z = vector.x * matrix.M31 + vector.y * matrix.M32 + vector.z * matrix.M33 + matrix.M34;
            result.w = vector.x * matrix.M41 + vector.y * matrix.M42 + vector.z * matrix.M43 + matrix.M44;
        }

        public static void Transform(ref FixVector4 vector, ref FixMatrix4x4 matrix, out FixVector4 result)
        {
            result.x = vector.x * matrix.M11 + vector.y * matrix.M12 + vector.z * matrix.M13 + vector.w * matrix.M14;
            result.y = vector.x * matrix.M21 + vector.y * matrix.M22 + vector.z * matrix.M23 + vector.w * matrix.M24;
            result.z = vector.x * matrix.M31 + vector.y * matrix.M32 + vector.z * matrix.M33 + vector.w * matrix.M34;
            result.w = vector.x * matrix.M41 + vector.y * matrix.M42 + vector.z * matrix.M43 + vector.w * matrix.M44;
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>Returns the dot product of both vectors.</returns>
        public static Fix64 Dot(FixVector4 vector1, FixVector4 vector2)
        {
            return FixVector4.Dot(ref vector1, ref vector2);
        }

        /// <summary>
        /// Calculates the dot product of both vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>Returns the dot product of both vectors.</returns>
        public static Fix64 Dot(ref FixVector4 vector1, ref FixVector4 vector2)
        {
            return ((vector1.x * vector2.x) + (vector1.y * vector2.y)) + (vector1.z * vector2.z) + (vector1.w * vector2.w);
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The sum of both vectors.</returns>
        public static FixVector4 Add(FixVector4 value1, FixVector4 value2)
        {
            FixVector4 result;
            FixVector4.Add(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Adds to vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The sum of both vectors.</param>
        public static void Add(ref FixVector4 value1, ref FixVector4 value2, out FixVector4 result)
        {
            result.x = value1.x + value2.x;
            result.y = value1.y + value2.y;
            result.z = value1.z + value2.z;
            result.w = value1.w + value2.w;
        }

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        public static FixVector4 Divide(FixVector4 value1, Fix64 scaleFactor)
        {
            FixVector4 result;
            FixVector4.Divide(ref value1, scaleFactor, out result);
            return result;
        }

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <param name="result">Returns the scaled vector.</param>
        public static void Divide(ref FixVector4 value1, Fix64 scaleFactor, out FixVector4 result)
        {
            result.x = value1.x / scaleFactor;
            result.y = value1.y / scaleFactor;
            result.z = value1.z / scaleFactor;
            result.w = value1.w / scaleFactor;
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The difference of both vectors.</returns>
        public static FixVector4 Subtract(FixVector4 value1, FixVector4 value2)
        {
            FixVector4 result;
            FixVector4.Subtract(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Subtracts to vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The difference of both vectors.</param>
        public static void Subtract(ref FixVector4 value1, ref FixVector4 value2, out FixVector4 result)
        {
            result.x = value1.x - value2.x;
            result.y = value1.y - value2.y;
            result.z = value1.z - value2.z;
            result.w = value1.w - value2.w;
        }

        public override bool Equals(object obj)
        {
            return obj is FixVector4 && (FixVector4)obj == this;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode() ^ w.GetHashCode();
        }

        public bool Equals(FixVector4 obj)
        {
            return this == obj;
        }

        /// <summary>
        /// Inverses the direction of the vector.
        /// </summary>
        public void Negate()
        {
            this.x = -this.x;
            this.y = -this.y;
            this.z = -this.z;
            this.w = -this.w;
        }

        /// <summary>
        /// Inverses the direction of a vector.
        /// </summary>
        /// <param name="value">The vector to inverse.</param>
        /// <returns>The negated vector.</returns>
        public static FixVector4 Negate(FixVector4 value)
        {
            FixVector4 result;
            FixVector4.Negate(ref value, out result);
            return result;
        }

        /// <summary>
        /// Inverses the direction of a vector.
        /// </summary>
        /// <param name="value">The vector to inverse.</param>
        /// <param name="result">The negated vector.</param>
        public static void Negate(ref FixVector4 value, out FixVector4 result)
        {
            result.x = -value.x;
            result.y = -value.y;
            result.z = -value.z;
            result.w = -value.w;
        }

        /// <summary>
        /// Normalizes the given vector.
        /// </summary>
        /// <param name="value">The vector which should be normalized.</param>
        /// <returns>A normalized vector.</returns>
        public static FixVector4 Normalize(FixVector4 value)
        {
            FixVector4 result;
            FixVector4.Normalize(ref value, out result);
            return result;
        }

        /// <summary>
        /// Normalizes this vector.
        /// </summary>
        public void Normalize()
        {
            Fix64 num2 = ((this.x * this.x) + (this.y * this.y)) + (this.z * this.z) + (this.w * this.w);
            Fix64 num = Fix64.One / Fix64.Sqrt(num2);
            this.x *= num;
            this.y *= num;
            this.z *= num;
            this.w *= num;
        }

        /// <summary>
        /// Normalizes the given vector.
        /// </summary>
        /// <param name="value">The vector which should be normalized.</param>
        /// <param name="result">A normalized vector.</param>
        public static void Normalize(ref FixVector4 value, out FixVector4 result)
        {
            Fix64 num2 = ((value.x * value.x) + (value.y * value.y)) + (value.z * value.z) + (value.w * value.w);
            Fix64 num = Fix64.One / Fix64.Sqrt(num2);
            result.x = value.x * num;
            result.y = value.y * num;
            result.z = value.z * num;
            result.w = value.w * num;
        }

        /// <summary>
        /// Swaps the components of both vectors.
        /// </summary>
        /// <param name="vector1">The first vector to swap with the second.</param>
        /// <param name="vector2">The second vector to swap with the first.</param>
        public static void Swap(ref FixVector4 vector1, ref FixVector4 vector2)
        {
            Fix64 temp;

            temp = vector1.x;
            vector1.x = vector2.x;
            vector2.x = temp;

            temp = vector1.y;
            vector1.y = vector2.y;
            vector2.y = temp;

            temp = vector1.z;
            vector1.z = vector2.z;
            vector2.z = temp;

            temp = vector1.w;
            vector1.w = vector2.w;
            vector2.w = temp;
        }

        /// <summary>
        /// Multiply a vector with a factor.
        /// </summary>
        /// <param name="value1">The vector to multiply.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the multiplied vector.</returns>
        public static FixVector4 Multiply(FixVector4 value1, Fix64 scaleFactor)
        {
            FixVector4 result;
            FixVector4.Multiply(ref value1, scaleFactor, out result);
            return result;
        }

        /// <summary>
        /// Multiply a vector with a factor.
        /// </summary>
        /// <param name="value1">The vector to multiply.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <param name="result">Returns the multiplied vector.</param>
        public static void Multiply(ref FixVector4 value1, Fix64 scaleFactor, out FixVector4 result)
        {
            result.x = value1.x * scaleFactor;
            result.y = value1.y * scaleFactor;
            result.z = value1.z * scaleFactor;
            result.w = value1.w * scaleFactor;
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>Returns the dot product of both.</returns>
        public static Fix64 operator *(FixVector4 value1, FixVector4 value2)
        {
            return FixVector4.Dot(ref value1, ref value2);
        }

        /// <summary>
        /// Multiplies a vector by a scale factor.
        /// </summary>
        /// <param name="value1">The vector to scale.</param>
        /// <param name="value2">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        public static FixVector4 operator *(FixVector4 value1, Fix64 value2)
        {
            FixVector4 result;
            FixVector4.Multiply(ref value1, value2, out result);
            return result;
        }

        /// <summary>
        /// Multiplies a vector by a scale factor.
        /// </summary>
        /// <param name="value2">The vector to scale.</param>
        /// <param name="value1">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        public static FixVector4 operator *(Fix64 value1, FixVector4 value2)
        {
            FixVector4 result;
            FixVector4.Multiply(ref value2, value1, out result);
            return result;
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The difference of both vectors.</returns>
        public static FixVector4 operator -(FixVector4 value1, FixVector4 value2)
        {
            FixVector4 result; 
            FixVector4.Subtract(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The sum of both vectors.</returns>
        public static FixVector4 operator +(FixVector4 value1, FixVector4 value2)
        {
            FixVector4 result; 
            FixVector4.Add(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        public static FixVector4 operator /(FixVector4 value1, Fix64 scaleFactor)
        {
            FixVector4 result;
            FixVector4.Divide(ref value1, scaleFactor, out result);
            return result;
        }

#if UNITY_5_3_OR_NEWER || UNITY_2017_1_OR_NEWER
        public static explicit operator FixVector4(UnityEngine.Vector4 vector4)
        {
            return new FixVector4((Fix64)vector4.x, (Fix64)vector4.y, (Fix64)vector4.z, (Fix64)vector4.w);
        }

        public static explicit operator UnityEngine.Vector4(FixVector4 vector4)
        {
            return new UnityEngine.Vector4((float)vector4.x, (float)vector4.y, (float)vector4.z, (float)vector4.w);
        }
#endif

        public FixVector2 ToVector2()
        {
            return new FixVector2(this.x, this.y);
        }

        public FixVector3 ToVector3()
        {
            return new FixVector3(this.x, this.y, this.z);
        }
    }
}
