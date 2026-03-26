namespace AstraEngine.Math
{
    public readonly struct Vector4
    {
        public Vector4(float x, float y, float z, float w)
        {
            X = x; Y = y; Z = z; W = w;
        }

        public float X { get; }
        public float Y { get; }
        public float Z { get; }
        public float W { get; }

        public static Vector4 Zero => new(0f, 0f, 0f, 0f);
        public static Vector4 One => new(1f, 1f, 1f, 1f);

        public float Length() => System.MathF.Sqrt(X * X + Y * Y + Z * Z + W * W);
        public float LengthSquared() => X * X + Y * Y + Z * Z + W * W;

        public static Vector4 operator +(Vector4 a, Vector4 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
        public static Vector4 operator -(Vector4 a, Vector4 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
        public static Vector4 operator -(Vector4 v) => new(-v.X, -v.Y, -v.Z, -v.W);
        public static Vector4 operator *(Vector4 v, float s) => new(v.X * s, v.Y * s, v.Z * s, v.W * s);
        public static Vector4 operator *(float s, Vector4 v) => v * s;
        public static Vector4 operator /(Vector4 v, float s) => new(v.X / s, v.Y / s, v.Z / s, v.W / s);

        public static float Dot(Vector4 a, Vector4 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;

        public static Vector4 Normalize(Vector4 v)
        {
            var len = v.Length();
            return len > 0f ? new Vector4(v.X / len, v.Y / len, v.Z / len, v.W / len) : Zero;
        }

        public static Vector4 Lerp(Vector4 a, Vector4 b, float t)
        {
            t = MathHelper.Clamp(t, 0f, 1f);
            return new Vector4(
                a.X + (b.X - a.X) * t,
                a.Y + (b.Y - a.Y) * t,
                a.Z + (b.Z - a.Z) * t,
                a.W + (b.W - a.W) * t);
        }

        public override string ToString() => $"({X}, {Y}, {Z}, {W})";
    }
}