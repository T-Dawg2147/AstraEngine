using AstraEngine.Graphics.Abstractions;

namespace AstraEngine.Graphics
{
    public sealed class GraphicsDeviceCreateInfo
    {
        public GraphicsBackend Backend { get; set; }
        public bool EnableDebugLayer { get; set; } = true;
    }

    public readonly struct BufferDescription
    {
        public BufferDescription(ulong sizeInBytes, BufferUsage usage, bool cpuAccessible = false)
        {
            SizeInBytes = sizeInBytes;
            Usage = usage;
            CpuAccessible = cpuAccessible;
        }

        public ulong SizeInBytes { get; }
        public BufferUsage Usage { get; }
        public bool CpuAccessible { get; }
    }

    public readonly struct TextureDescription
    {
        public TextureDescription(int width, int height, PixelFormat format, TextureUsage usage)
        {
            Width = width;
            Height = height;
            Format = format;
            Usage = usage;
        }

        public int Width { get; }
        public int Height { get; }
        public PixelFormat Format { get; }
        public TextureUsage Usage { get; }
    }

    public readonly struct ShaderDescription
    {
        public ShaderDescription(ShaderStage stage, string source)
        {
            Stage = stage;
            Source = source;
        }

        public ShaderStage Stage { get; }
        public string Source { get; }
    }

    public readonly struct PipelineDescription
    {
        public PipelineDescription(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
