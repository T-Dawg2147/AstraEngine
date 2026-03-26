namespace AstraEngine.Math
{
    public readonly struct Color4
    {
        public Color4(float r, float g, float b, float a = 1f)
        {
            R = r; G = g; B = b; A = a;
        }

        public float R { get; }
        public float G { get; }
        public float B { get; }
        public float A { get; }

        public static Color4 Black => new(0f, 0f, 0f, 1f);
        public static Color4 White => new(1f, 1f, 1f, 1f);
        public static Color4 Red => new(1f, 0f, 0f, 1f);
        public static Color4 Green => new(0f, 1f, 0f, 1f);
        public static Color4 Blue => new(0f, 0f, 1f, 1f);
        public static Color4 Yellow => new(1f, 1f, 0f, 1f);
        public static Color4 Cyan => new(0f, 1f, 1f, 1f);
        public static Color4 Magenta => new(1f, 0f, 1f, 1f);
        public static Color4 Transparent => new(0f, 0f, 0f, 0f);
        public static Color4 CornflowerBlue => new(0.392f, 0.584f, 0.929f, 1f);

        public static Color4 operator +(Color4 a, Color4 b) => new(a.R + b.R, a.G + b.G, a.B + b.B, a.A + b.A);
        public static Color4 operator -(Color4 a, Color4 b) => new(a.R - b.R, a.G - b.G, a.B - b.B, a.A - b.A);
        public static Color4 operator *(Color4 c, float s) => new(c.R * s, c.G * s, c.B * s, c.A * s);
        public static Color4 operator *(float s, Color4 c) => c * s;
        public static Color4 operator *(Color4 a, Color4 b) => new(a.R * b.R, a.G * b.G, a.B * b.B, a.A * b.A);

        public static Color4 Lerp(Color4 a, Color4 b, float t)
        {
            t = MathHelper.Clamp(t, 0f, 1f);
            return new Color4(
                a.R + (b.R - a.R) * t,
                a.G + (b.G - a.G) * t,
                a.B + (b.B - a.B) * t,
                a.A + (b.A - a.A) * t);
        }

        public Color4 Clamped() => new(
            MathHelper.Clamp(R, 0f, 1f),
            MathHelper.Clamp(G, 0f, 1f),
            MathHelper.Clamp(B, 0f, 1f),
            MathHelper.Clamp(A, 0f, 1f));

        public override string ToString() => $"({R}, {G}, {B}, {A})";
    }
}