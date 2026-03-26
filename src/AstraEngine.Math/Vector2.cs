namespace AstraEngine.Math
{
    public readonly struct Vector2
    {
        public Vector2(float x, float y)
        {
            X = x; Y = y;
        }

        public float X { get; }
        public float Y { get; }

        public static Vector2 Zero => new(0f, 0f);
        public static Vector2 One => new(1f, 1f);
        public static Vector2 UnitX => new(1f, 0f);
        public static Vector2 UnitY => new(0f, 1f);

        public float Length() => System.MathF.Sqrt(X * X + Y * Y);
        public float LengthSquared() => X * X + Y * Y;

        public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.X - b.X, a.Y - b.Y);
        public static Vector2 operator -(Vector2 v) => new(-v.X, -v.Y);
        public static Vector2 operator *(Vector2 v, float s) => new(v.X * s, v.Y * s);
        public static Vector2 operator *(float s, Vector2 v) => v * s;
        public static Vector2 operator /(Vector2 v, float s) => new(v.X / s, v.Y / s);

        public static float Dot(Vector2 a, Vector2 b) => a.X * b.X + a.Y * b.Y;

        public static Vector2 Normalize(Vector2 v)
        {
            var len = v.Length();
            return len > 0f ? new Vector2(v.X / len, v.Y / len) : Zero;
        }

        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            t = MathHelper.Clamp(t, 0f, 1f);
            return new Vector2(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t);
        }

        public override string ToString() => $"({X}, {Y})";
    }
}