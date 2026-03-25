namespace AstraEngine.Graphics.OpenGL
{
    public sealed class OpenGLPipeline : IPipeline
    {
        public OpenGLPipeline(PipelineDescription description)
        {
            Description = description;
        }

        public PipelineDescription Description { get; }

        public void Dispose() { }
    }
}
