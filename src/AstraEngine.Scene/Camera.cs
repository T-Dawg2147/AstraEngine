using AstraEngine.Math;

namespace AstraEngine.Scene
{
    public sealed class Camera
    {
        public Vector3 Position { get; set; } = new(0f, 0f, -5f);
        public float Pitch { get; set; }
        public float Yaw { get; set; } = 90f;

        public float FieldOfView { get; set; } = 60f;
        public float NearPlane { get; set; } = 0.1f;
        public float FarPlane { get; set; } = 1000f;

        public Matrix4x4 ViewMatrix => Matrix4x4.CreateLookAt(Position, Position + Forward, Up);

        public Matrix4x4 ProjectionMatrix(float aspectRatio)
            => Matrix4x4.CreatePerspectiveFieldOfView(FieldOfView * (System.MathF.PI / 180f), aspectRatio, NearPlane, FarPlane);

        public Vector3 Forward
        {
            get
            {
                var yawRad = Yaw * (System.MathF.PI / 180f);
                var pitchRad = Pitch * (System.MathF.PI / 180f);

                var x = System.MathF.Cos(yawRad) * System.MathF.Cos(pitchRad);
                var y = System.MathF.Sin(pitchRad);
                var z = System.MathF.Sin(yawRad) * System.MathF.Cos(pitchRad);

                return Vector3.Normalize(new Vector3(x, y, z));
            }
        }

        public Vector3 Right => Vector3.Normalize(Vector3.Cross(Forward, Up));
        public Vector3 Up => new(0f, 1f, 0f);
    }
}