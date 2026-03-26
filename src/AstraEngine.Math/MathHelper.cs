namespace AstraEngine.Math
{
    public static class MathHelper
    {
        public const float Pi = 3.14159265358979323846f;
        public const float TwoPi = Pi * 2f;
        public const float HalfPi = Pi * 0.5f;
        public const float Deg2Rad = Pi / 180f;
        public const float Rad2Deg = 180f / Pi;

        public static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static float Lerp(float a, float b, float t)
            => a + (b - a) * Clamp(t, 0f, 1f);

        public static float InverseLerp(float a, float b, float value)
        {
            if (System.MathF.Abs(b - a) < float.Epsilon) return 0f;
            return Clamp((value - a) / (b - a), 0f, 1f);
        }

        public static float DegreesToRadians(float degrees) => degrees * Deg2Rad;
        public static float RadiansToDegrees(float radians) => radians * Rad2Deg;

        public static float Min(float a, float b) => a < b ? a : b;
        public static float Max(float a, float b) => a > b ? a : b;

        public static float Abs(float value) => System.MathF.Abs(value);

        public static float Sqrt(float value) => System.MathF.Sqrt(value);

        public static float Sin(float value) => System.MathF.Sin(value);
        public static float Cos(float value) => System.MathF.Cos(value);
        public static float Tan(float value) => System.MathF.Tan(value);

        public static float Atan2(float y, float x) => System.MathF.Atan2(y, x);

        public static bool ApproximatelyEqual(float a, float b, float epsilon = 1e-6f)
            => System.MathF.Abs(a - b) < epsilon;
    }
}
