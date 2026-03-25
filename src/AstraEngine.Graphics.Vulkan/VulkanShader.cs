namespace AstraEngine.Graphics.Vulkan
{
    public sealed class VulkanShader : IShader
    {
        public VulkanShader(ShaderDescription description)
        {
            Description = description;
        }

        public ShaderDescription Description { get; }

        public void Dispose() { }
    }
}
