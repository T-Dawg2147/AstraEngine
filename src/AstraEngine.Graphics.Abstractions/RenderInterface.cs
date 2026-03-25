using AstraEngine.Graphics.Abstractions;
using AstraEngine.Math;
using AstraEngine.Platform;

namespace AstraEngine.Graphics
{
    public interface ITexture2D : IDisposable
    {
        int Width { get; }
        int Height { get; }
    }

    public interface ITexture : IDisposable
    {
        TextureDescription Description { get; }
        int Width { get; }
        int Height { get; }
        PixelFormat Format { get; }
    }

    public interface IRenderDevice : IDisposable
    {
        GraphicsBackend Backend { get; }

        ISwapChain CreateSwapChain(IWindow window);
        IBuffer CreateBuffer(BufferDescription description);
        ITexture CreateTexture(TextureDescription description);
        IShader CreateShader(ShaderDescription description);
        IPipeline CreatePipeline(PipelineDescription description);
        ICommandList CreateCommandList();
    }

    public interface ISwapChain : IDisposable
    {
        int Width { get; }
        int Height { get; }

        void Present();
        void Resize(int width, int height);
    }

    public interface IBuffer : IDisposable
    {
        ulong SizeInBytes { get; }
    }

    public interface IShader : IDisposable { }

    public interface IPipeline : IDisposable { }

    public interface ICommandList : IDisposable
    {
        void Begin();
        void End();

        void ClearColor(ISwapChain swapChain, Color4 color);
        void Draw(int vertexCount, int startVertextLocation = 0);
        void DrawIndexed(int indexCount, int startIndexLocation = 0, int baseVertextLocation = 0);
    }
}
