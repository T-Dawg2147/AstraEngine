namespace AstraEngine.Math
{
    public readonly struct Vector3
    {
        public Vector3(float x, float y, float z)
        {
            X = x; Y = y; Z = z;
        }

        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public static Vector3 Zero => new(0f, 0f, 0f);
        public static Vector3 One => new(1f, 1f, 1f);
        public static Vector3 UnitX => new(1f, 0f, 0f);
        public static Vector3 UnitY => new(0f, 1f, 0f);
        public static Vector3 UnitZ => new(0f, 0f, 1f);

        public float Length() => System.MathF.Sqrt(X * X + Y * Y + Z * Z);
        public float LengthSquared() => X * X + Y * Y + Z * Z;

        public static Vector3 operator +(Vector3 a, Vector3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vector3 operator -(Vector3 a, Vector3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Vector3 operator -(Vector3 v) => new(-v.X, -v.Y, -v.Z);
        public static Vector3 operator *(Vector3 v, float s) => new(v.X * s, v.Y * s, v.Z * s);
        public static Vector3 operator *(float s, Vector3 v) => v * s;
        public static Vector3 operator /(Vector3 v, float s) => new(v.X / s, v.Y / s, v.Z / s);

        public static float Dot(Vector3 a, Vector3 b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

        public static Vector3 Cross(Vector3 a, Vector3 b) => new(
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X);

        public static Vector3 Normalize(Vector3 v)
        {
            var len = System.MathF.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);
            return len > 0f ? new Vector3(v.X / len, v.Y / len, v.Z / len) : Zero;
        }

        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            t = MathHelper.Clamp(t, 0f, 1f);
            return new Vector3(
                a.X + (b.X - a.X) * t,
                a.Y + (b.Y - a.Y) * t,
                a.Z + (b.Z - a.Z) * t);
        }

        public static float Distance(Vector3 a, Vector3 b)
            => (a - b).Length();

        public static float DistanceSquared(Vector3 a, Vector3 b)
            => (a - b).LengthSquared();

        public static Vector3 Reflect(Vector3 direction, Vector3 normal)
            => direction - normal * (2f * Dot(direction, normal));

        public static Vector3 Min(Vector3 a, Vector3 b)
            => new(System.MathF.Min(a.X, b.X), System.MathF.Min(a.Y, b.Y), System.MathF.Min(a.Z, b.Z));

        public static Vector3 Max(Vector3 a, Vector3 b)
            => new(System.MathF.Max(a.X, b.X), System.MathF.Max(a.Y, b.Y), System.MathF.Max(a.Z, b.Z));

        public override string ToString() => $"({X}, {Y}, {Z})";
    }
}