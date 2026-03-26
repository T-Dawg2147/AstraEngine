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

        public float Length() => System.MathF.Sqrt(X * X + Y * Y + Z * Z + W * W);
        public float LengthSquared() => X * X + Y * Y + Z * Z + W * W;

        public static Quaternion Normalize(Quaternion q)
        {
            var len = q.Length();
            return len > 0f ? new Quaternion(q.X / len, q.Y / len, q.Z / len, q.W / len) : Identity;
        }

        public static Quaternion Conjugate(Quaternion q)
            => new(-q.X, -q.Y, -q.Z, q.W);

        public static Quaternion Inverse(Quaternion q)
        {
            var lenSq = q.LengthSquared();
            if (lenSq < float.Epsilon) return Identity;
            var inv = 1f / lenSq;
            return new Quaternion(-q.X * inv, -q.Y * inv, -q.Z * inv, q.W * inv);
        }

        public static Quaternion operator *(Quaternion a, Quaternion b) => new(
            a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
            a.W * b.Y - a.X * b.Z + a.Y * b.W + a.Z * b.X,
            a.W * b.Z + a.X * b.Y - a.Y * b.X + a.Z * b.W,
            a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z);

        public static Quaternion CreateFromAxisAngle(Vector3 axis, float angleRadians)
        {
            var half = angleRadians * 0.5f;
            var s = System.MathF.Sin(half);
            var c = System.MathF.Cos(half);
            var n = Vector3.Normalize(axis);
            return new Quaternion(n.X * s, n.Y * s, n.Z * s, c);
        }

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

        public static Quaternion Slerp(Quaternion a, Quaternion b, float t)
        {
            t = MathHelper.Clamp(t, 0f, 1f);
            var dot = a.X * b.X + a.Y * b.Y + a.Z * b.Z + a.W * b.W;

            if (dot < 0f)
            {
                b = new Quaternion(-b.X, -b.Y, -b.Z, -b.W);
                dot = -dot;
            }

            if (dot > 0.9995f)
            {
                return Normalize(new Quaternion(
                    a.X + (b.X - a.X) * t,
                    a.Y + (b.Y - a.Y) * t,
                    a.Z + (b.Z - a.Z) * t,
                    a.W + (b.W - a.W) * t));
            }

            var theta = System.MathF.Acos(dot);
            var sinTheta = System.MathF.Sin(theta);
            var wa = System.MathF.Sin((1f - t) * theta) / sinTheta;
            var wb = System.MathF.Sin(t * theta) / sinTheta;

            return new Quaternion(
                wa * a.X + wb * b.X,
                wa * a.Y + wb * b.Y,
                wa * a.Z + wb * b.Z,
                wa * a.W + wb * b.W);
        }

        public Vector3 Rotate(Vector3 v)
        {
            var qv = new Quaternion(v.X, v.Y, v.Z, 0f);
            var conj = Conjugate(this);
            var result = this * qv * conj;
            return new Vector3(result.X, result.Y, result.Z);
        }

        public override string ToString() => $"({X}, {Y}, {Z}, {W})";
    }
}