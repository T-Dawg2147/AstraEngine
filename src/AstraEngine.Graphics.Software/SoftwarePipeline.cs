namespace AstraEngine.Graphics.Software
{
    public sealed class SoftwarePipeline : IPipeline
    {
        public SoftwarePipeline(PipelineDescription description)
        {
            Description = description;
        }

        public PipelineDescription Description { get; }
        
        public void Dispose() { }
    }
}
