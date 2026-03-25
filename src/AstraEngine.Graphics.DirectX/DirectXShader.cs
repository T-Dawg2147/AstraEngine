namespace AstraEngine.Graphics.DirectX
{
    public sealed class DirectXShader : IShader
    {
        public DirectXShader(ShaderDescription description)
        {
            Description = description;
        }

        public ShaderDescription Description { get; }

        public void Dispose() { }
    }
}
