using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstraEngine.Math
{
    public readonly struct Quaternion
    {
        public Quaternion(float x, float y, float z, float w)
        {
            X = x; Y = y; Z = z; W = w;
        }

        public float X { get; }
        public float Y { get; }
        public float Z { get; }
        public float W { get; }

        public static Quaternion Identity => new(0f, 0f, 0f, 1f);

        public static Quaternion CreateFromYawPitchRoll(float yaw, float pitch, float roll)
        {
            var halfRoll = roll * 0.5f;
            var halfPitch = pitch * 0.5f;
            var halfYaw = yaw * 0.5f;

            var sinRoll = System.MathF.Sin(halfRoll);
            var cosRoll = System.MathF.Cos(halfRoll);
            var sinPitch = System.MathF.Sin(halfPitch);
            var cosPitch = System.MathF.Cos(halfPitch);
            var sinYaw = System.MathF.Sin(halfYaw);
            var cosYaw = System.MathF.Cos(halfYaw);

            return new Quaternion(
                (cosYaw * sinPitch * cosRoll) + (sinYaw * cosPitch * sinRoll),
                (sinYaw * cosPitch * cosRoll) - (cosYaw * sinPitch * sinRoll),
                (cosYaw * cosPitch * sinRoll) - (sinYaw * sinPitch * cosRoll),
                (cosYaw * cosPitch * cosRoll) + (sinYaw * sinPitch * sinRoll));
        }
    }
}
