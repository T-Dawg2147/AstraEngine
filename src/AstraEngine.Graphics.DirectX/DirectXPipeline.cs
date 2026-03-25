namespace AstraEngine.Graphics.DirectX
{
    public sealed class DirectXPipeline : IPipeline
    {
        public DirectXPipeline(PipelineDescription description)
        {
            Description = description;
        }

        public PipelineDescription Description { get; }

        public void Dispose() { }
    }
}
