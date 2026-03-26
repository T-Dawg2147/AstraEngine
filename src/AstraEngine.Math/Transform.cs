namespace AstraEngine.Math
{
    public struct Transform
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;

        public Transform()
        {
            Position = Vector3.Zero;
            Rotation = Quaternion.Identity;
            Scale = Vector3.One;
        }

        public Matrix4x4 LocalToWorldMatrix
        {
            get
            {
                var scale = Matrix4x4.CreateScale(Scale);
                var rotation = Matrix4x4.CreateRotation(Rotation);
                var translation = Matrix4x4.CreateTranslation(Position);
                return scale * rotation * translation;
            }
        }
    }
}