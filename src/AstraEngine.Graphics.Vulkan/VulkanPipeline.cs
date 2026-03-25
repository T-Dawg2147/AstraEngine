namespace AstraEngine.Graphics.Vulkan
{
    public sealed class VulkanPipeline : IPipeline
    {
        public VulkanPipeline(PipelineDescription description)
        {
            Description = description;
        }

        public PipelineDescription Description { get; }
       
        public void Dispose() { }
    }
}
