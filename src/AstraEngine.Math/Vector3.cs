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

        public static Vector3 operator +(Vector3 a, Vector3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vector3 operator -(Vector3 a, Vector3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Vector3 operator *(Vector3 v, float s) => new(v.X * s, v.Y * s, v.Z * s);
        public static Vector3 operator *(float s, Vector3 v) => v * s;

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
    }
}
