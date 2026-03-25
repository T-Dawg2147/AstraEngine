namespace AstraEngine.Graphics.OpenGL
{
    public sealed class OpenGLShader : IShader
    {
        public OpenGLShader(ShaderDescription description)
        {
            Description = description;
        }

        public ShaderDescription Description { get; }

        public void Dispose() { }
    }
}
