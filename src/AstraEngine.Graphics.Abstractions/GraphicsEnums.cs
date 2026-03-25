namespace AstraEngine.Graphics.Abstractions
{
    public enum GraphicsBackend
    {
        Software, 
        OpenGL,
        Vulkan,
        DirectX
    }

    public enum PixelFormat
    {
        Rgba8,
        Bgra8,
        Depth24Stencil8,
        Rgba16Float,
        R8G8B8A8,
        B8G8R8A8
    }

    public enum BufferUsage
    {
        Vertex,
        Index,
        Constant,
        Storage,
        Staging
    }

    public enum TextureUsage
    {
        Sampled, 
        RenderTarget,
        DepthStencil,
        Storage
    }

    public enum ShaderStage
    {
        Vertex, 
        Fragment,
        Compute
    }
}
