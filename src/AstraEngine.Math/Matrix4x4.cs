using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstraEngine.Math
{
    public readonly struct Matrix4x4
    {
        public Matrix4x4(
            float m11, float m12, float m13, float m14,
            float m21, float m22, float m23, float m24,
            float m31, float m32, float m33, float m34,
            float m41, float m42, float m43, float m44)
        {
            M11 = m11; M12 = m12; M13 = m13; M14 = m14;
            M21 = m21; M22 = m22; M23 = m23; M24 = m24;
            M31 = m31; M32 = m32; M33 = m33; M34 = m34;
            M41 = m41; M42 = m42; M43 = m43; M44 = m44;
        }

        public float M11 { get; }
        public float M12 { get; }
        public float M13 { get; }
        public float M14 { get; }
        
        public float M21 { get; }
        public float M22 { get; }
        public float M23 { get; }
        public float M24 { get; }

        public float M31 { get; }
        public float M32 { get; }
        public float M33 { get; }
        public float M34 { get; }

        public float M41 { get; }
        public float M42 { get; }
        public float M43 { get; }
        public float M44 { get; }

        public static Matrix4x4 Identity => new(
            1f, 0f, 0f, 0f,
            0f, 1f, 0f, 0f,
            0f, 0f, 1f, 0f,
            0f, 0f, 0f, 1f);

        public static Matrix4x4 CreateTranslation(Vector3 translation) => new(
            1f, 0f, 0f, translation.X,
            0f, 1f, 0f, translation.Y,
            0f, 0f, 1f, translation.Z,
            0f, 0f, 0f, 1f);

        public static Matrix4x4 CreateScale(Vector3 scale) => new(
            scale.X, 0f, 0f, 0f,
            0f, scale.Y, 0f, 0f,
            0f, 0f, scale.Z, 0f,
            0f, 0f, 0f, 1f);

        public static Matrix4x4 CreateRotation(Quaternion q)
        {
            var xx = q.X * q.X;
            var yy = q.Y * q.Y;
            var zz = q.Z * q.Z;
            var xy = q.X * q.Y;
            var xz = q.X * q.Z;
            var yz = q.Y * q.Z;
            var wx = q.W * q.X;
            var wy = q.W * q.Y;
            var wz = q.W * q.Z;

            return new Matrix4x4(
                1f - 2f * (yy + zz), 2f * (xy - wx), 2f * (xz + wy), 0f,
                2f * (xy + wz), 1f - 2f * (xx + zz), 2f * (yz - wx), 0f,
                2f * (xz - wy), 2f * (yz + wx), 1f - 2f * (xx + yy), 0f,
                0f, 0f, 0f, 1f);
        }

        public static Matrix4x4 CreatePerspectiveFieldOfView(float fovY, float aspectRatio, float nearPlane, float farPlane)
        {
            var f = 1f / System.MathF.Tan(fovY * 0.5f);
            var rangeInv = 1f / (nearPlane - farPlane);

            return new Matrix4x4(
                f / aspectRatio, 0f, 0f, 0f,
                0f, f, 0f, 0f,
                0f, 0f, (farPlane + nearPlane) * rangeInv, 2f * farPlane * nearPlane * rangeInv,
                0f, 0f, -1f, 0f);
        }

        public static Matrix4x4 CreateLookAt(Vector3 eye, Vector3 target, Vector3 up)
        {
            var zaxis = Normalize(eye - target);
            var xaxis = Normalize(Cross(up, zaxis));
            var yaxis = Cross(zaxis, xaxis);

            return new Matrix4x4(
                xaxis.X, yaxis.X, zaxis.X, 0f,
                xaxis.Y, yaxis.Y, zaxis.Y, 0f,
                xaxis.Z, yaxis.Z, zaxis.Z, 0f,
                -Dot(xaxis, eye), -Dot(yaxis, eye), -Dot(zaxis, eye), 1f);
        }

        private static float Dot(Vector3 a, Vector3 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

        private static Vector3 Cross(Vector3 a, Vector3 b) => new(
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X);

        private static Vector3 Normalize(Vector3 v)
        {
            var len = System.MathF.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);
            return len > 0f ? new Vector3(v.X / len, v.Y / len, v.Z / len) : Vector3.Zero;
        }

        public static Matrix4x4 operator *(Matrix4x4 a, Matrix4x4 b)
        {
            return new Matrix4x4(
                a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31 + a.M14 * b.M41,
                a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32 + a.M14 * b.M42,
                a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33 + a.M14 * b.M43,
                a.M11 * b.M14 + a.M12 * b.M24 + a.M13 * b.M34 + a.M14 * b.M44,

                a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31 + a.M24 * b.M41,
                a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32 + a.M24 * b.M42,
                a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33 + a.M24 * b.M43,
                a.M21 * b.M14 + a.M22 * b.M24 + a.M23 * b.M34 + a.M24 * b.M44,

                a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31 + a.M34 * b.M41,
                a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32 + a.M34 * b.M42,
                a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33 + a.M34 * b.M43,
                a.M31 * b.M14 + a.M32 * b.M24 + a.M33 * b.M34 + a.M34 * b.M44,

                a.M41 * b.M11 + a.M42 * b.M21 + a.M43 * b.M31 + a.M44 * b.M41,
                a.M41 * b.M12 + a.M42 * b.M22 + a.M43 * b.M32 + a.M44 * b.M42,
                a.M41 * b.M13 + a.M42 * b.M23 + a.M43 * b.M33 + a.M44 * b.M43,
                a.M41 * b.M14 + a.M42 * b.M24 + a.M43 * b.M34 + a.M44 * b.M44);
        }

        public Vector4 Transform(Vector4 v)
        {
            return new Vector4(
                (M11 * v.X) + (M12 * v.Y) + (M13 * v.Z) + (M14 * v.W),
                (M21 * v.X) + (M22 * v.Y) + (M23 * v.Z) + (M24 * v.W),
                (M31 * v.X) + (M32 * v.Y) + (M33 * v.Z) + (M34 * v.W),
                (M41 * v.X) + (M42 * v.Y) + (M43 * v.Z) + (M44 * v.W));
        }
    }
}
