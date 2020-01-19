﻿using MessagePack;
using System;

namespace Common
{
    [MessagePackObject]
    public struct FixMatrix3x3 : IEquatable<FixMatrix3x3>
    {
        [Key(0)]
        public Fix64 M11; // 1st row vector
        [Key(1)]
        public Fix64 M12;
        [Key(2)]
        public Fix64 M13;
        [Key(3)]
        public Fix64 M21; // 2nd row vector
        [Key(4)]
        public Fix64 M22;
        [Key(5)]
        public Fix64 M23;
        [Key(6)]
        public Fix64 M31; // 3rd row vector
        [Key(7)]
        public Fix64 M32;
        [Key(8)]
        public Fix64 M33;

        internal static FixMatrix3x3 InternalIdentity;

        /// <summary>
        /// Identity matrix.
        /// </summary>
        public static readonly FixMatrix3x3 Identity;
        public static readonly FixMatrix3x3 Zero;

        static FixMatrix3x3()
        {
            Zero = new FixMatrix3x3();

            Identity = new FixMatrix3x3();
            Identity.M11 = Fix64.One;
            Identity.M22 = Fix64.One;
            Identity.M33 = Fix64.One;

            InternalIdentity = Identity;
        }

        [IgnoreMember]
        public FixVector3 eulerAngles
        {
            get
            {
                FixVector3 result = new FixVector3();

                result.x = FixMath.Atan2(M32, M33) * Fix64.Rad2Deg;
                result.y = FixMath.Atan2(-M31, FixMath.Sqrt(M32 * M32 + M33 * M33)) * Fix64.Rad2Deg;
                result.z = FixMath.Atan2(M21, M11) * Fix64.Rad2Deg;

                return result * -1;
            }
        }

        public static FixMatrix3x3 CreateFromYawPitchRoll(Fix64 yaw, Fix64 pitch, Fix64 roll)
        {
            FixMatrix3x3 matrix;
            FixQuaternion quaternion;
            FixQuaternion.CreateFromYawPitchRoll(yaw, pitch, roll, out quaternion);
            CreateFromQuaternion(ref quaternion, out matrix);
            return matrix;
        }

        public static FixMatrix3x3 CreateRotationX(Fix64 radians)
        {
            FixMatrix3x3 matrix;
            Fix64 num2 = Fix64.Cos(radians);
            Fix64 num = Fix64.Sin(radians);
            matrix.M11 = Fix64.One;
            matrix.M12 = Fix64.Zero;
            matrix.M13 = Fix64.Zero;
            matrix.M21 = Fix64.Zero;
            matrix.M22 = num2;
            matrix.M23 = num;
            matrix.M31 = Fix64.Zero;
            matrix.M32 = -num;
            matrix.M33 = num2;
            return matrix;
        }

        public static void CreateRotationX(Fix64 radians, out FixMatrix3x3 result)
        {
            Fix64 num2 = Fix64.Cos(radians);
            Fix64 num = Fix64.Sin(radians);
            result.M11 = Fix64.One;
            result.M12 = Fix64.Zero;
            result.M13 = Fix64.Zero;
            result.M21 = Fix64.Zero;
            result.M22 = num2;
            result.M23 = num;
            result.M31 = Fix64.Zero;
            result.M32 = -num;
            result.M33 = num2;
        }

        public static FixMatrix3x3 CreateRotationY(Fix64 radians)
        {
            FixMatrix3x3 matrix;
            Fix64 num2 = Fix64.Cos(radians);
            Fix64 num = Fix64.Sin(radians);
            matrix.M11 = num2;
            matrix.M12 = Fix64.Zero;
            matrix.M13 = -num;
            matrix.M21 = Fix64.Zero;
            matrix.M22 = Fix64.One;
            matrix.M23 = Fix64.Zero;
            matrix.M31 = num;
            matrix.M32 = Fix64.Zero;
            matrix.M33 = num2;
            return matrix;
        }

        public static void CreateRotationY(Fix64 radians, out FixMatrix3x3 result)
        {
            Fix64 num2 = Fix64.Cos(radians);
            Fix64 num = Fix64.Sin(radians);
            result.M11 = num2;
            result.M12 = Fix64.Zero;
            result.M13 = -num;
            result.M21 = Fix64.Zero;
            result.M22 = Fix64.One;
            result.M23 = Fix64.Zero;
            result.M31 = num;
            result.M32 = Fix64.Zero;
            result.M33 = num2;
        }

        public static FixMatrix3x3 CreateRotationZ(Fix64 radians)
        {
            FixMatrix3x3 matrix;
            Fix64 num2 = Fix64.Cos(radians);
            Fix64 num = Fix64.Sin(radians);
            matrix.M11 = num2;
            matrix.M12 = num;
            matrix.M13 = Fix64.Zero;
            matrix.M21 = -num;
            matrix.M22 = num2;
            matrix.M23 = Fix64.Zero;
            matrix.M31 = Fix64.Zero;
            matrix.M32 = Fix64.Zero;
            matrix.M33 = Fix64.One;
            return matrix;
        }


        public static void CreateRotationZ(Fix64 radians, out FixMatrix3x3 result)
        {
            Fix64 num2 = Fix64.Cos(radians);
            Fix64 num = Fix64.Sin(radians);
            result.M11 = num2;
            result.M12 = num;
            result.M13 = Fix64.Zero;
            result.M21 = -num;
            result.M22 = num2;
            result.M23 = Fix64.Zero;
            result.M31 = Fix64.Zero;
            result.M32 = Fix64.Zero;
            result.M33 = Fix64.One;
        }

        /// <summary>
        /// Initializes a new instance of the matrix structure.
        /// </summary>
        /// <param name="m11">m11</param>
        /// <param name="m12">m12</param>
        /// <param name="m13">m13</param>
        /// <param name="m21">m21</param>
        /// <param name="m22">m22</param>
        /// <param name="m23">m23</param>
        /// <param name="m31">m31</param>
        /// <param name="m32">m32</param>
        /// <param name="m33">m33</param>
        public FixMatrix3x3(Fix64 m11, Fix64 m12, Fix64 m13, Fix64 m21, Fix64 m22, Fix64 m23, Fix64 m31, Fix64 m32, Fix64 m33)
        {
            this.M11 = m11;
            this.M12 = m12;
            this.M13 = m13;
            this.M21 = m21;
            this.M22 = m22;
            this.M23 = m23;
            this.M31 = m31;
            this.M32 = m32;
            this.M33 = m33;
        }

        /// <summary>
        /// Gets the determinant of the matrix.
        /// </summary>
        /// <returns>The determinant of the matrix.</returns>
        #region public Fix64 Determinant()
        //public Fix64 Determinant()
        //{
        //    return M11 * M22 * M33 -M11 * M23 * M32 -M12 * M21 * M33 +M12 * M23 * M31 + M13 * M21 * M32 - M13 * M22 * M31;
        //}
        #endregion

        /// <summary>
        /// Multiply two matrices. Notice: matrix multiplication is not commutative.
        /// </summary>
        /// <param name="matrix1">The first matrix.</param>
        /// <param name="matrix2">The second matrix.</param>
        /// <returns>The product of both matrices.</returns>
        public static FixMatrix3x3 Multiply(FixMatrix3x3 matrix1, FixMatrix3x3 matrix2)
        {
            FixMatrix3x3 result;
            FixMatrix3x3.Multiply(ref matrix1, ref matrix2, out result);
            return result;
        }

        /// <summary>
        /// Multiply two matrices. Notice: matrix multiplication is not commutative.
        /// </summary>
        /// <param name="matrix1">The first matrix.</param>
        /// <param name="matrix2">The second matrix.</param>
        /// <param name="result">The product of both matrices.</param>
        public static void Multiply(ref FixMatrix3x3 matrix1, ref FixMatrix3x3 matrix2, out FixMatrix3x3 result)
        {
            Fix64 num0 = ((matrix1.M11 * matrix2.M11) + (matrix1.M12 * matrix2.M21)) + (matrix1.M13 * matrix2.M31);
            Fix64 num1 = ((matrix1.M11 * matrix2.M12) + (matrix1.M12 * matrix2.M22)) + (matrix1.M13 * matrix2.M32);
            Fix64 num2 = ((matrix1.M11 * matrix2.M13) + (matrix1.M12 * matrix2.M23)) + (matrix1.M13 * matrix2.M33);
            Fix64 num3 = ((matrix1.M21 * matrix2.M11) + (matrix1.M22 * matrix2.M21)) + (matrix1.M23 * matrix2.M31);
            Fix64 num4 = ((matrix1.M21 * matrix2.M12) + (matrix1.M22 * matrix2.M22)) + (matrix1.M23 * matrix2.M32);
            Fix64 num5 = ((matrix1.M21 * matrix2.M13) + (matrix1.M22 * matrix2.M23)) + (matrix1.M23 * matrix2.M33);
            Fix64 num6 = ((matrix1.M31 * matrix2.M11) + (matrix1.M32 * matrix2.M21)) + (matrix1.M33 * matrix2.M31);
            Fix64 num7 = ((matrix1.M31 * matrix2.M12) + (matrix1.M32 * matrix2.M22)) + (matrix1.M33 * matrix2.M32);
            Fix64 num8 = ((matrix1.M31 * matrix2.M13) + (matrix1.M32 * matrix2.M23)) + (matrix1.M33 * matrix2.M33);

            result.M11 = num0;
            result.M12 = num1;
            result.M13 = num2;
            result.M21 = num3;
            result.M22 = num4;
            result.M23 = num5;
            result.M31 = num6;
            result.M32 = num7;
            result.M33 = num8;
        }

        /// <summary>
        /// Matrices are added.
        /// </summary>
        /// <param name="matrix1">The first matrix.</param>
        /// <param name="matrix2">The second matrix.</param>
        /// <returns>The sum of both matrices.</returns>
        public static FixMatrix3x3 Add(FixMatrix3x3 matrix1, FixMatrix3x3 matrix2)
        {
            FixMatrix3x3 result;
            FixMatrix3x3.Add(ref matrix1, ref matrix2, out result);
            return result;
        }

        /// <summary>
        /// Matrices are added.
        /// </summary>
        /// <param name="matrix1">The first matrix.</param>
        /// <param name="matrix2">The second matrix.</param>
        /// <param name="result">The sum of both matrices.</param>
        public static void Add(ref FixMatrix3x3 matrix1, ref FixMatrix3x3 matrix2, out FixMatrix3x3 result)
        {
            result.M11 = matrix1.M11 + matrix2.M11;
            result.M12 = matrix1.M12 + matrix2.M12;
            result.M13 = matrix1.M13 + matrix2.M13;
            result.M21 = matrix1.M21 + matrix2.M21;
            result.M22 = matrix1.M22 + matrix2.M22;
            result.M23 = matrix1.M23 + matrix2.M23;
            result.M31 = matrix1.M31 + matrix2.M31;
            result.M32 = matrix1.M32 + matrix2.M32;
            result.M33 = matrix1.M33 + matrix2.M33;
        }

        /// <summary>
        /// Calculates the inverse of a give matrix.
        /// </summary>
        /// <param name="matrix">The matrix to invert.</param>
        /// <returns>The inverted FixMatrix3x3.</returns>
        public static FixMatrix3x3 Inverse(FixMatrix3x3 matrix)
        {
            FixMatrix3x3 result;
            FixMatrix3x3.Inverse(ref matrix, out result);
            return result;
        }

        public Fix64 Determinant()
        {
            return M11 * M22 * M33 + M12 * M23 * M31 + M13 * M21 * M32 -
                   M31 * M22 * M13 - M32 * M23 * M11 - M33 * M21 * M12;
        }

        public static void Invert(ref FixMatrix3x3 matrix, out FixMatrix3x3 result)
        {
            Fix64 determinantInverse = 1 / matrix.Determinant();
            Fix64 m11 = (matrix.M22 * matrix.M33 - matrix.M23 * matrix.M32) * determinantInverse;
            Fix64 m12 = (matrix.M13 * matrix.M32 - matrix.M33 * matrix.M12) * determinantInverse;
            Fix64 m13 = (matrix.M12 * matrix.M23 - matrix.M22 * matrix.M13) * determinantInverse;

            Fix64 m21 = (matrix.M23 * matrix.M31 - matrix.M21 * matrix.M33) * determinantInverse;
            Fix64 m22 = (matrix.M11 * matrix.M33 - matrix.M13 * matrix.M31) * determinantInverse;
            Fix64 m23 = (matrix.M13 * matrix.M21 - matrix.M11 * matrix.M23) * determinantInverse;

            Fix64 m31 = (matrix.M21 * matrix.M32 - matrix.M22 * matrix.M31) * determinantInverse;
            Fix64 m32 = (matrix.M12 * matrix.M31 - matrix.M11 * matrix.M32) * determinantInverse;
            Fix64 m33 = (matrix.M11 * matrix.M22 - matrix.M12 * matrix.M21) * determinantInverse;

            result.M11 = m11;
            result.M12 = m12;
            result.M13 = m13;

            result.M21 = m21;
            result.M22 = m22;
            result.M23 = m23;

            result.M31 = m31;
            result.M32 = m32;
            result.M33 = m33;
        }

        /// <summary>
        /// Calculates the inverse of a give matrix.
        /// </summary>
        /// <param name="matrix">The matrix to invert.</param>
        /// <param name="result">The inverted FixMatrix3x3.</param>
        public static void Inverse(ref FixMatrix3x3 matrix, out FixMatrix3x3 result)
        {
            Fix64 det = 1024 * matrix.M11 * matrix.M22 * matrix.M33 -
                1024 * matrix.M11 * matrix.M23 * matrix.M32 -
                1024 * matrix.M12 * matrix.M21 * matrix.M33 +
                1024 * matrix.M12 * matrix.M23 * matrix.M31 +
                1024 * matrix.M13 * matrix.M21 * matrix.M32 -
                1024 * matrix.M13 * matrix.M22 * matrix.M31;

            Fix64 num11 = 1024 * matrix.M22 * matrix.M33 - 1024 * matrix.M23 * matrix.M32;
            Fix64 num12 = 1024 * matrix.M13 * matrix.M32 - 1024 * matrix.M12 * matrix.M33;
            Fix64 num13 = 1024 * matrix.M12 * matrix.M23 - 1024 * matrix.M22 * matrix.M13;

            Fix64 num21 = 1024 * matrix.M23 * matrix.M31 - 1024 * matrix.M33 * matrix.M21;
            Fix64 num22 = 1024 * matrix.M11 * matrix.M33 - 1024 * matrix.M31 * matrix.M13;
            Fix64 num23 = 1024 * matrix.M13 * matrix.M21 - 1024 * matrix.M23 * matrix.M11;

            Fix64 num31 = 1024 * matrix.M21 * matrix.M32 - 1024 * matrix.M31 * matrix.M22;
            Fix64 num32 = 1024 * matrix.M12 * matrix.M31 - 1024 * matrix.M32 * matrix.M11;
            Fix64 num33 = 1024 * matrix.M11 * matrix.M22 - 1024 * matrix.M21 * matrix.M12;

            if (det == 0)
            {
                result.M11 = Fix64.PositiveInfinity;
                result.M12 = Fix64.PositiveInfinity;
                result.M13 = Fix64.PositiveInfinity;
                result.M21 = Fix64.PositiveInfinity;
                result.M22 = Fix64.PositiveInfinity;
                result.M23 = Fix64.PositiveInfinity;
                result.M31 = Fix64.PositiveInfinity;
                result.M32 = Fix64.PositiveInfinity;
                result.M33 = Fix64.PositiveInfinity;
            }
            else
            {
                result.M11 = num11 / det;
                result.M12 = num12 / det;
                result.M13 = num13 / det;
                result.M21 = num21 / det;
                result.M22 = num22 / det;
                result.M23 = num23 / det;
                result.M31 = num31 / det;
                result.M32 = num32 / det;
                result.M33 = num33 / det;
            }

        }

        /// <summary>
        /// Multiply a matrix by a scalefactor.
        /// </summary>
        /// <param name="matrix1">The matrix.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <returns>A FixMatrix3x3 multiplied by the scale factor.</returns>
        public static FixMatrix3x3 Multiply(FixMatrix3x3 matrix1, Fix64 scaleFactor)
        {
            FixMatrix3x3 result;
            FixMatrix3x3.Multiply(ref matrix1, scaleFactor, out result);
            return result;
        }

        /// <summary>
        /// Multiply a matrix by a scalefactor.
        /// </summary>
        /// <param name="matrix1">The matrix.</param>
        /// <param name="scaleFactor">The scale factor.</param>
        /// <param name="result">A FixMatrix3x3 multiplied by the scale factor.</param>
        public static void Multiply(ref FixMatrix3x3 matrix1, Fix64 scaleFactor, out FixMatrix3x3 result)
        {
            Fix64 num = scaleFactor;
            result.M11 = matrix1.M11 * num;
            result.M12 = matrix1.M12 * num;
            result.M13 = matrix1.M13 * num;
            result.M21 = matrix1.M21 * num;
            result.M22 = matrix1.M22 * num;
            result.M23 = matrix1.M23 * num;
            result.M31 = matrix1.M31 * num;
            result.M32 = matrix1.M32 * num;
            result.M33 = matrix1.M33 * num;
        }

        /// <summary>
        /// Creates a FixMatrix3x3 representing an orientation from a quaternion.
        /// </summary>
        /// <param name="quaternion">The quaternion the matrix should be created from.</param>
        /// <returns>FixMatrix3x3 representing an orientation.</returns>
        public static FixMatrix3x3 CreateFromLookAt(FixVector3 position, FixVector3 target)
        {
            FixMatrix3x3 result;
            LookAt(target - position, FixVector3.up, out result);
            return result;
        }

        public static FixMatrix3x3 LookAt(FixVector3 forward, FixVector3 upwards)
        {
            FixMatrix3x3 result;
            LookAt(forward, upwards, out result);

            return result;
        }

        public static void LookAt(FixVector3 forward, FixVector3 upwards, out FixMatrix3x3 result)
        {
            FixVector3 zaxis = forward; zaxis.Normalize();
            FixVector3 xaxis = FixVector3.Cross(upwards, zaxis); xaxis.Normalize();
            FixVector3 yaxis = FixVector3.Cross(zaxis, xaxis);

            result.M11 = xaxis.x;
            result.M21 = yaxis.x;
            result.M31 = zaxis.x;
            result.M12 = xaxis.y;
            result.M22 = yaxis.y;
            result.M32 = zaxis.y;
            result.M13 = xaxis.z;
            result.M23 = yaxis.z;
            result.M33 = zaxis.z;
        }

        public static FixMatrix3x3 CreateFromQuaternion(FixQuaternion quaternion)
        {
            FixMatrix3x3 result;
            FixMatrix3x3.CreateFromQuaternion(ref quaternion, out result);
            return result;
        }

        /// <summary>
        /// Creates a FixMatrix3x3 representing an orientation from a quaternion.
        /// </summary>
        /// <param name="quaternion">The quaternion the matrix should be created from.</param>
        /// <param name="result">FixMatrix3x3 representing an orientation.</param>
        public static void CreateFromQuaternion(ref FixQuaternion quaternion, out FixMatrix3x3 result)
        {
            Fix64 num9 = quaternion.x * quaternion.x;
            Fix64 num8 = quaternion.y * quaternion.y;
            Fix64 num7 = quaternion.z * quaternion.z;
            Fix64 num6 = quaternion.x * quaternion.y;
            Fix64 num5 = quaternion.z * quaternion.w;
            Fix64 num4 = quaternion.z * quaternion.x;
            Fix64 num3 = quaternion.y * quaternion.w;
            Fix64 num2 = quaternion.y * quaternion.z;
            Fix64 num = quaternion.x * quaternion.w;
            result.M11 = Fix64.One - (2 * (num8 + num7));
            result.M12 = 2 * (num6 + num5);
            result.M13 = 2 * (num4 - num3);
            result.M21 = 2 * (num6 - num5);
            result.M22 = Fix64.One - (2 * (num7 + num9));
            result.M23 = 2 * (num2 + num);
            result.M31 = 2 * (num4 + num3);
            result.M32 = 2 * (num2 - num);
            result.M33 = Fix64.One - (2 * (num8 + num9));
        }

        /// <summary>
        /// Creates the transposed matrix.
        /// </summary>
        /// <param name="matrix">The matrix which should be transposed.</param>
        /// <returns>The transposed FixMatrix3x3.</returns>
        public static FixMatrix3x3 Transpose(FixMatrix3x3 matrix)
        {
            FixMatrix3x3 result;
            FixMatrix3x3.Transpose(ref matrix, out result);
            return result;
        }

        /// <summary>
        /// Creates the transposed matrix.
        /// </summary>
        /// <param name="matrix">The matrix which should be transposed.</param>
        /// <param name="result">The transposed FixMatrix3x3.</param>
        public static void Transpose(ref FixMatrix3x3 matrix, out FixMatrix3x3 result)
        {
            result.M11 = matrix.M11;
            result.M12 = matrix.M21;
            result.M13 = matrix.M31;
            result.M21 = matrix.M12;
            result.M22 = matrix.M22;
            result.M23 = matrix.M32;
            result.M31 = matrix.M13;
            result.M32 = matrix.M23;
            result.M33 = matrix.M33;
        }

        /// <summary>
        /// Multiplies two matrices.
        /// </summary>
        /// <param name="value1">The first matrix.</param>
        /// <param name="value2">The second matrix.</param>
        /// <returns>The product of both values.</returns>
        public static FixMatrix3x3 operator *(FixMatrix3x3 value1, FixMatrix3x3 value2)
        {
            FixMatrix3x3 result; FixMatrix3x3.Multiply(ref value1, ref value2, out result);
            return result;
        }

        public Fix64 Trace()
        {
            return this.M11 + this.M22 + this.M33;
        }

        /// <summary>
        /// Adds two matrices.
        /// </summary>
        /// <param name="value1">The first matrix.</param>
        /// <param name="value2">The second matrix.</param>
        /// <returns>The sum of both values.</returns>
        public static FixMatrix3x3 operator +(FixMatrix3x3 value1, FixMatrix3x3 value2)
        {
            FixMatrix3x3 result; FixMatrix3x3.Add(ref value1, ref value2, out result);
            return result;
        }

        /// <summary>
        /// Subtracts two matrices.
        /// </summary>
        /// <param name="value1">The first matrix.</param>
        /// <param name="value2">The second matrix.</param>
        /// <returns>The difference of both values.</returns>
        public static FixMatrix3x3 operator -(FixMatrix3x3 value1, FixMatrix3x3 value2)
        {
            FixMatrix3x3 result; FixMatrix3x3.Multiply(ref value2, -Fix64.One, out value2);
            FixMatrix3x3.Add(ref value1, ref value2, out result);
            return result;
        }

        public static bool operator ==(FixMatrix3x3 value1, FixMatrix3x3 value2)
        {
            return value1.M11 == value2.M11 &&
                value1.M12 == value2.M12 &&
                value1.M13 == value2.M13 &&
                value1.M21 == value2.M21 &&
                value1.M22 == value2.M22 &&
                value1.M23 == value2.M23 &&
                value1.M31 == value2.M31 &&
                value1.M32 == value2.M32 &&
                value1.M33 == value2.M33;
        }

        public static bool operator !=(FixMatrix3x3 value1, FixMatrix3x3 value2)
        {
            return value1.M11 != value2.M11 ||
                value1.M12 != value2.M12 ||
                value1.M13 != value2.M13 ||
                value1.M21 != value2.M21 ||
                value1.M22 != value2.M22 ||
                value1.M23 != value2.M23 ||
                value1.M31 != value2.M31 ||
                value1.M32 != value2.M32 ||
                value1.M33 != value2.M33;
        }

        public override bool Equals(object obj)
        {
            return obj is FixMatrix3x3 && (FixMatrix3x3)obj == this;
        }

        public override int GetHashCode()
        {
            return M11.GetHashCode() ^
                M12.GetHashCode() ^
                M13.GetHashCode() ^
                M21.GetHashCode() ^
                M22.GetHashCode() ^
                M23.GetHashCode() ^
                M31.GetHashCode() ^
                M32.GetHashCode() ^
                M33.GetHashCode();
        }

        public bool Equals(FixMatrix3x3 other)
        {
            return this == other;
        }

        /// <summary>
        /// Creates a matrix which rotates around the given axis by the given angle.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="angle">The angle.</param>
        /// <param name="result">The resulting rotation matrix</param>
        public static void CreateFromAxisAngle(ref FixVector3 axis, Fix64 angle, out FixMatrix3x3 result)
        {
            Fix64 x = axis.x;
            Fix64 y = axis.y;
            Fix64 z = axis.z;
            Fix64 num2 = Fix64.Sin(angle);
            Fix64 num = Fix64.Cos(angle);
            Fix64 num11 = x * x;
            Fix64 num10 = y * y;
            Fix64 num9 = z * z;
            Fix64 num8 = x * y;
            Fix64 num7 = x * z;
            Fix64 num6 = y * z;
            result.M11 = num11 + (num * (Fix64.One - num11));
            result.M12 = (num8 - (num * num8)) + (num2 * z);
            result.M13 = (num7 - (num * num7)) - (num2 * y);
            result.M21 = (num8 - (num * num8)) - (num2 * z);
            result.M22 = num10 + (num * (Fix64.One - num10));
            result.M23 = (num6 - (num * num6)) + (num2 * x);
            result.M31 = (num7 - (num * num7)) + (num2 * y);
            result.M32 = (num6 - (num * num6)) - (num2 * x);
            result.M33 = num9 + (num * (Fix64.One - num9));
        }

        /// <summary>
        /// Creates a matrix which rotates around the given axis by the given angle.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="angle">The angle.</param>
        /// <returns>The resulting rotation matrix</returns>
        public static FixMatrix3x3 AngleAxis(Fix64 angle, FixVector3 axis)
        {
            FixMatrix3x3 result; CreateFromAxisAngle(ref axis, angle, out result);
            return result;
        }

        public override string ToString()
        {
            return string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}", M11.RawValue, M12.RawValue, M13.RawValue, M21.RawValue, M22.RawValue, M23.RawValue, M31.RawValue, M32.RawValue, M33.RawValue);
        }
    }
}
