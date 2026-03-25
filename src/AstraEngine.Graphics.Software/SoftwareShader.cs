namespace AstraEngine.Graphics.Software
{
    public sealed class SoftwareShader : IShader
    {
        public SoftwareShader(ShaderDescription description)
        {
            Description = description;
        }

        public ShaderDescription Description { get; }

        public void Dispose() { }
    }
}
