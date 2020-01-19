using MessagePack;
using System;

namespace Common
{
    [MessagePackObject]
    public struct FixVector3 : IEquatable<FixVector3>
    {
        private static Fix64 ZeroEpsilonSq = FixMath.Epsilon;
        internal static FixVector3 InternalZero;
        internal static FixVector3 Arbitrary;

        [Key(0)]
        public Fix64 x;
        [Key(1)]
        public Fix64 y;
        [Key(2)]
        public Fix64 z;

        #region Static readonly variables
        /// <summary>
        /// A vector with components (0,0,0);
        /// </summary>
        public static readonly FixVector3 zero;
        /// <summary>
        /// A vector with components (-1,0,0);
        /// </summary>
        public static readonly FixVector3 left;
        /// <summary>
        /// A vector with components (1,0,0);
        /// </summary>
        public static readonly FixVector3 right;
        /// <summary>
        /// A vector with components (0,1,0);
        /// </summary>
        public static readonly FixVector3 up;
        /// <summary>
        /// A vector with components (0,-1,0);
        /// </summary>
        public static readonly FixVector3 down;
        /// <summary>
        /// A vector with components (0,0,-1);
        /// </summary>
        public static readonly FixVector3 back;
        /// <summary>
        /// A vector with components (0,0,1);
        /// </summary>
        public static readonly FixVector3 forward;
        /// <summary>
        /// A vector with components (1,1,1);
        /// </summary>
        public static readonly FixVector3 one;
        /// <summary>
        /// A vector with components 
        /// (Fix64.MinValue,Fix64.MinValue,Fix64.MinValue);
        /// </summary>
        public static readonly FixVector3 MinValue;
        /// <summary>
        /// A vector with components 
        /// (Fix64.MaxValue,Fix64.MaxValue,Fix64.MaxValue);
        /// </summary>
        public static readonly FixVector3 MaxValue;
        #endregion

        #region Private static constructor
        static FixVector3()
        {
            one = new FixVector3(1, 1, 1);
            zero = new FixVector3(0, 0, 0);
            left = new FixVector3(-1, 0, 0);
            right = new FixVector3(1, 0, 0);
            up = new FixVector3(0, 1, 0);
            down = new FixVector3(0, -1, 0);
            back = new FixVector3(0, 0, -1);
            forward = new FixVector3(0, 0, 1);
            MinValue = new FixVector3(Fix64.MinValue);
            MaxValue = new FixVector3(Fix64.MaxValue);
            Arbitrary = new FixVector3(1, 1, 1);
            InternalZero = zero;
        }
        #endregion

        public static FixVector3 Abs(FixVector3 other)
        {
            return new FixVector3(Fix64.Abs(other.x), Fix64.Abs(other.y), Fix64.Abs(other.z));
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
                return (((this.x * this.x) + (this.y * this.y)) + (this.z * this.z));
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
                Fix64 num = ((this.x * this.x) + (this.y * this.y)) + (this.z * this.z);
                return Fix64.Sqrt(num);
            }
        }

        public static FixVector3 ClampMagnitude(FixVector3 vector, Fix64 maxLength)
        {
            return Normalize(vector) * maxLength;
        }

        /// <summary>
        /// Gets a normalized version of the vector.
        /// </summary>
        /// <returns>Returns a normalized version of the vector.</returns>
        [IgnoreMember]
        public FixVector3 normalized
        {
            get
            {
                FixVector3 result = new FixVector3(this.x, this.y, this.z);
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

        public FixVector3(int x, int y, int z)
        {
            this.x = (Fix64)x;
            this.y = (Fix64)y;
            this.z = (Fix64)z;
        }

        public FixVector3(Fix64 x, Fix64 y, Fix64 z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Multiplies each component of the vector by the same components of the provided vector.
        /// </summary>
        public void Scale(FixVector3 other)
        {
            this.x = x * other.x;
            this.y = y * other.y;
            this.z = z * other.z;
        }

        /// <summary>
        /// Sets all vector component to specific values.
        /// </summary>
        /// <param name="x">The X component of the vector.</param>
        /// <param name="y">The Y component of the vector.</param>
        /// <param name="z">The Z component of the vector.</param>
        public void Set(Fix64 x, Fix64 y, Fix64 z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Constructor initializing a new instance of the structure
        /// </summary>
        /// <param name="xyz">All components of the vector are set to xyz</param>
        public FixVector3(Fix64 xyz)
        {
            this.x = xyz;
            this.y = xyz;
            this.z = xyz;
        }

        public static FixVector3 Lerp(FixVector3 from, FixVector3 to, Fix64 percent)
        {
            return from + (to - from) * percent;
        }

        /// <summary>
        /// Builds a string from the FixVector3.
        /// </summary>
        /// <returns>A string containing all three components.</returns>
        #region public override string ToString()
        public override string ToString()
        {
            return string.Format("({0:f1}, {1:f1}, {2:f1})", x.AsFloat(), y.AsFloat(), z.AsFloat());
        }
        #endregion

        /// <summary>
        /// Multiplies each component of the vector by the same components of the provided vector.
        /// </summary>
        public static FixVector3 Scale(FixVector3 vecA, FixVector3 vecB)
        {
            FixVector3 result;
            result.x = vecA.x * vecB.x;
            result.y = vecA.y * vecB.y;
            result.z = vecA.z * vecB.z;

            return result;
        }

        /// <summary>
        /// Tests if two FixVector3 are equal.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>Returns true if both values are equal, otherwise false.</returns>
        public static bool operator ==(FixVector3 value1, FixVector3 value2)
        {
            return (((value1.x == value2.x) && (value1.y == value2.y)) && (value1.z == value2.z));
        }

        /// <summary>
        /// Tests if two FixVector3 are not equal.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>Returns false if both values are equal, otherwise true.</returns>
        public static bool operator !=(FixVector3 value1, FixVector3 value2)
        {
            if ((value1.x == value2.x) && (value1.y == value2.y))
            {
                return (value1.z != value2.z);
            }
            return true;
        }

        /// <summary>
        /// Gets a vector with the minimum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>A vector with the minimum x,y and z values of both vectors.</returns>
        public static FixVector3 Min(FixVector3 value1, FixVector3 value2)
        {
            FixVector3 result;
            FixVector3.Min(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Gets a vector with the minimum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="result">A vector with the minimum x,y and z values of both vectors.</param>
        public static void Min(ref FixVector3 value1, ref FixVector3 value2, out FixVector3 result)
        {
            result.x = (value1.x < value2.x) ? value1.x : value2.x;
            result.y = (value1.y < value2.y) ? value1.y : value2.y;
            result.z = (value1.z < value2.z) ? value1.z : value2.z;
        }

        /// <summary>
        /// Gets a vector with the maximum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <returns>A vector with the maximum x,y and z values of both vectors.</returns>
        public static FixVector3 Max(FixVector3 value1, FixVector3 value2)
        {
            FixVector3 result;
            FixVector3.Max(ref value1, ref value2, out result);
            return result;
        }

        public static Fix64 Distance(FixVector3 v1, FixVector3 v2)
        {
            return Fix64.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y) + (v1.z - v2.z) * (v1.z - v2.z));
        }

        /// <summary>
        /// Gets a vector with the maximum x,y and z values of both vectors.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="result">A vector with the maximum x,y and z values of both vectors.</param>
        public static void Max(ref FixVector3 value1, ref FixVector3 value2, out FixVector3 result)
        {
            result.x = (value1.x > value2.x) ? value1.x : value2.x;
            result.y = (value1.y > value2.y) ? value1.y : value2.y;
            result.z = (value1.z > value2.z) ? value1.z : value2.z;
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
        public static FixVector3 Transform(FixVector3 position, FixMatrix3x3 matrix)
        {
            FixVector3 result;
            FixVector3.Transform(ref position, ref matrix, out result);
            return result;
        }

        /// <summary>
        /// Transforms a vector by the given matrix.
        /// </summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transform matrix.</param>
        /// <param name="result">The transformed vector.</param>
        public static void Transform(ref FixVector3 position, ref FixMatrix3x3 matrix, out FixVector3 result)
        {
            Fix64 num0 = ((position.x * matrix.M11) + (position.y * matrix.M21)) + (position.z * matrix.M31);
            Fix64 num1 = ((position.x * matrix.M12) + (position.y * matrix.M22)) + (position.z * matrix.M32);
            Fix64 num2 = ((position.x * matrix.M13) + (position.y * matrix.M23)) + (position.z * matrix.M33);

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }

        /// <summary>
        /// Transforms a vector by the transposed of the given Matrix.
        /// </summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transform matrix.</param>
        /// <param name="result">The transformed vector.</param>
        public static void TransposedTransform(ref FixVector3 position, ref FixMatrix3x3 matrix, out FixVector3 result)
        {
            Fix64 num0 = ((position.x * matrix.M11) + (position.y * matrix.M12)) + (position.z * matrix.M13);
            Fix64 num1 = ((position.x * matrix.M21) + (position.y * matrix.M22)) + (position.z * matrix.M23);
            Fix64 num2 = ((position.x * matrix.M31) + (position.y * matrix.M32)) + (position.z * matrix.M33);

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>Returns the dot product of both vectors.</returns>
        public static Fix64 Dot(FixVector3 vector1, FixVector3 vector2)
        {
            return FixVector3.Dot(ref vector1, ref vector2);
        }


        /// <summary>
        /// Calculates the dot product of both vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>Returns the dot product of both vectors.</returns>
        public static Fix64 Dot(ref FixVector3 vector1, ref FixVector3 vector2)
        {
            return ((vector1.x * vector2.x) + (vector1.y * vector2.y)) + (vector1.z * vector2.z);
        }

        // Projects a vector onto another vector.
        public static FixVector3 Project(FixVector3 vector, FixVector3 onNormal)
        {
            Fix64 sqrtMag = Dot(onNormal, onNormal);
            if (sqrtMag < FixMath.Epsilon)
                return zero;
            else
                return onNormal * Dot(vector, onNormal) / sqrtMag;
        }

        // Projects a vector onto a plane defined by a normal orthogonal to the plane.
        public static FixVector3 ProjectOnPlane(FixVector3 vector, FixVector3 planeNormal)
        {
            return vector - Project(vector, planeNormal);
        }


        // Returns the angle in degrees between /from/ and /to/. This is always the smallest
        public static Fix64 Angle(FixVector3 from, FixVector3 to)
        {
            return FixMath.Acos(FixMath.Clamp(Dot(from.normalized, to.normalized), -Fix64.ONE, Fix64.ONE)) * FixMath.Rad2Deg;
        }

        // The smaller of the two possible angles between the two vectors is returned, therefore the result will never be greater than 180 degrees or smaller than -180 degrees.
        // If you imagine the from and to vectors as lines on a piece of paper, both originating from the same point, then the /axis/ vector would point up out of the paper.
        // The measured angle between the two vectors would be positive in a clockwise direction and negative in an anti-clockwise direction.
        public static Fix64 SignedAngle(FixVector3 from, FixVector3 to, FixVector3 axis)
        {
            FixVector3 fromNorm = from.normalized, toNorm = to.normalized;
            Fix64 unsignedAngle = FixMath.Acos(FixMath.Clamp(Dot(fromNorm, toNorm), -Fix64.ONE, Fix64.ONE)) * FixMath.Rad2Deg;
            Fix64 sign = FixMath.Sign(Dot(axis, Cross(fromNorm, toNorm)));
            return unsignedAngle * sign;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The sum of both vectors.</returns>
        public static FixVector3 Add(FixVector3 value1, FixVector3 value2)
        {
            FixVector3 result;
            FixVector3.Add(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Adds to vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The sum of both vectors.</param>
        public static void Add(ref FixVector3 value1, ref FixVector3 value2, out FixVector3 result)
        {
            Fix64 num0 = value1.x + value2.x;
            Fix64 num1 = value1.y + value2.y;
            Fix64 num2 = value1.z + value2.z;

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        public static FixVector3 Divide(FixVector3 value1, Fix64 scaleFactor)
        {
            FixVector3 result;
            FixVector3.Divide(ref value1, scaleFactor, out result);
            return result;
        }

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <param name="result">Returns the scaled vector.</param>
        public static void Divide(ref FixVector3 value1, Fix64 scaleFactor, out FixVector3 result)
        {
            result.x = value1.x / scaleFactor;
            result.y = value1.y / scaleFactor;
            result.z = value1.z / scaleFactor;
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The difference of both vectors.</returns>
        public static FixVector3 Subtract(FixVector3 value1, FixVector3 value2)
        {
            FixVector3 result;
            FixVector3.Subtract(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Subtracts to vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">The difference of both vectors.</param>
        public static void Subtract(ref FixVector3 value1, ref FixVector3 value2, out FixVector3 result)
        {
            Fix64 num0 = value1.x - value2.x;
            Fix64 num1 = value1.y - value2.y;
            Fix64 num2 = value1.z - value2.z;

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }

        /// <summary>
        /// The cross product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>The cross product of both vectors.</returns>
        public static FixVector3 Cross(FixVector3 vector1, FixVector3 vector2)
        {
            FixVector3 result;
            FixVector3.Cross(ref vector1, ref vector2, out result);
            return result;
        }

        /// <summary>
        /// The cross product of two vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <param name="result">The cross product of both vectors.</param>
        public static void Cross(ref FixVector3 vector1, ref FixVector3 vector2, out FixVector3 result)
        {
            Fix64 num3 = (vector1.y * vector2.z) - (vector1.z * vector2.y);
            Fix64 num2 = (vector1.z * vector2.x) - (vector1.x * vector2.z);
            Fix64 num = (vector1.x * vector2.y) - (vector1.y * vector2.x);
            result.x = num3;
            result.y = num2;
            result.z = num;
        }

        public override bool Equals(object obj)
        {
            return obj is FixVector3 && (FixVector3)obj == this;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }

        public bool Equals(FixVector3 obj)
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
        }

        /// <summary>
        /// Inverses the direction of a vector.
        /// </summary>
        /// <param name="value">The vector to inverse.</param>
        /// <returns>The negated vector.</returns>
        public static FixVector3 Negate(FixVector3 value)
        {
            FixVector3 result;
            FixVector3.Negate(ref value, out result);
            return result;
        }

        /// <summary>
        /// Inverses the direction of a vector.
        /// </summary>
        /// <param name="value">The vector to inverse.</param>
        /// <param name="result">The negated vector.</param>
        public static void Negate(ref FixVector3 value, out FixVector3 result)
        {
            Fix64 num0 = -value.x;
            Fix64 num1 = -value.y;
            Fix64 num2 = -value.z;

            result.x = num0;
            result.y = num1;
            result.z = num2;
        }

        /// <summary>
        /// Normalizes the given vector.
        /// </summary>
        /// <param name="value">The vector which should be normalized.</param>
        /// <returns>A normalized vector.</returns>
        public static FixVector3 Normalize(FixVector3 value)
        {
            FixVector3 result;
            FixVector3.Normalize(ref value, out result);
            return result;
        }

        /// <summary>
        /// Normalizes this vector.
        /// </summary>
        public void Normalize()
        {
            Fix64 num2 = ((this.x * this.x) + (this.y * this.y)) + (this.z * this.z);
            Fix64 num = Fix64.One / Fix64.Sqrt(num2);
            this.x *= num;
            this.y *= num;
            this.z *= num;
        }

        /// <summary>
        /// Normalizes the given vector.
        /// </summary>
        /// <param name="value">The vector which should be normalized.</param>
        /// <param name="result">A normalized vector.</param>
        public static void Normalize(ref FixVector3 value, out FixVector3 result)
        {
            Fix64 num2 = ((value.x * value.x) + (value.y * value.y)) + (value.z * value.z);
            Fix64 num = Fix64.One / Fix64.Sqrt(num2);
            result.x = value.x * num;
            result.y = value.y * num;
            result.z = value.z * num;
        }

        /// <summary>
        /// Swaps the components of both vectors.
        /// </summary>
        /// <param name="vector1">The first vector to swap with the second.</param>
        /// <param name="vector2">The second vector to swap with the first.</param>
        public static void Swap(ref FixVector3 vector1, ref FixVector3 vector2)
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
        }

        /// <summary>
        /// Multiply a vector with a factor.
        /// </summary>
        /// <param name="value1">The vector to multiply.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the multiplied vector.</returns>
        public static FixVector3 Multiply(FixVector3 value1, Fix64 scaleFactor)
        {
            FixVector3 result;
            FixVector3.Multiply(ref value1, scaleFactor, out result);
            return result;
        }

        /// <summary>
        /// Multiply a vector with a factor.
        /// </summary>
        /// <param name="value1">The vector to multiply.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <param name="result">Returns the multiplied vector.</param>
        public static void Multiply(ref FixVector3 value1, Fix64 scaleFactor, out FixVector3 result)
        {
            result.x = value1.x * scaleFactor;
            result.y = value1.y * scaleFactor;
            result.z = value1.z * scaleFactor;
        }

        /// <summary>
        /// Calculates the cross product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>Returns the cross product of both.</returns>
        public static FixVector3 operator %(FixVector3 value1, FixVector3 value2)
        {
            FixVector3 result; 
            FixVector3.Cross(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>Returns the dot product of both.</returns>
        public static Fix64 operator *(FixVector3 value1, FixVector3 value2)
        {
            return FixVector3.Dot(ref value1, ref value2);
        }

        /// <summary>
        /// Multiplies a vector by a scale factor.
        /// </summary>
        /// <param name="value1">The vector to scale.</param>
        /// <param name="value2">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        public static FixVector3 operator *(FixVector3 value1, Fix64 value2)
        {
            FixVector3 result;
            FixVector3.Multiply(ref value1, value2, out result);
            return result;
        }

        /// <summary>
        /// Multiplies a vector by a scale factor.
        /// </summary>
        /// <param name="value2">The vector to scale.</param>
        /// <param name="value1">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        public static FixVector3 operator *(Fix64 value1, FixVector3 value2)
        {
            FixVector3 result;
            FixVector3.Multiply(ref value2, value1, out result);
            return result;
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The difference of both vectors.</returns>
        public static FixVector3 operator -(FixVector3 value1, FixVector3 value2)
        {
            FixVector3 result; 
            FixVector3.Subtract(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The sum of both vectors.</returns>
        public static FixVector3 operator +(FixVector3 value1, FixVector3 value2)
        {
            FixVector3 result; 
            FixVector3.Add(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Divides a vector by a factor.
        /// </summary>
        /// <param name="value1">The vector to divide.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>Returns the scaled vector.</returns>
        public static FixVector3 operator /(FixVector3 value1, Fix64 scaleFactor)
        {
            FixVector3 result;
            FixVector3.Divide(ref value1, scaleFactor, out result);
            return result;
        }

#if UNITY_5_3_OR_NEWER || UNITY_2017_1_OR_NEWER
        public static explicit operator FixVector3(UnityEngine.Vector3 vector3)
        {
            return new FixVector3((Fix64)vector3.x, (Fix64)vector3.y, (Fix64)vector3.z);
        }

        public static explicit operator UnityEngine.Vector3(FixVector3 vector3)
        {
            return new UnityEngine.Vector3((float)vector3.x, (float)vector3.y, (float)vector3.z);
        }
#endif

        public FixVector2 ToVector2()
        {
            return new FixVector2(this.x, this.y);
        }

        public FixVector4 ToVector4()
        {
            return new FixVector4(this.x, this.y, this.z, Fix64.One);
        }
    }
}
