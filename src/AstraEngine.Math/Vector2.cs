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

        public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.X - b.X, a.Y - b.Y);
        public static Vector2 operator *(Vector2 v, float s) => new(v.X * s, v.Y * s);
    }
}
