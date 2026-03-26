using System.Runtime.InteropServices;
using AstraEngine.Math;
using AstraEngine.Platform;
using AstraEngine.Platform.Windows;

namespace AstraEngine.Graphics.Software
{
    public sealed class SoftwareFrameBuffer
    {
        private int _width;
        private int _height;
        private int[] _pixels;
        private float[] _depth;

        public SoftwareFrameBuffer(int width, int height)
        {
            _width = System.Math.Max(1, width);
            _height = System.Math.Max(1, height);
            _pixels = new int[_width * _height];
            _depth = new float[_width * _height];
            ClearDepth();
        }

        public int Width => _width;
        public int Height => _height;
        public Span<int> Pixels => _pixels;
        public Span<float> Depth => _depth;

        public void Resize(int width, int height)
        {
            _width = System.Math.Max(1, width);
            _height = System.Math.Max(1, height);
            _pixels = new int[_width * _height];
            _depth = new float[_width * _height];
            ClearDepth();
        }

        public void Clear(Color4 color)
        {
            var rgba = ToBgra32(color);
            Array.Fill(_pixels, rgba);
            ClearDepth();
        }

        public void ClearDepth()
        {
            Array.Fill(_depth, float.PositiveInfinity);
        }

        public void PresentToWindow(IWindow window)
            => window.Present(_pixels, _width, _height);

        private static int ToBgra32(Color4 c)
        {
            static byte Clamp(float v) => (byte)System.Math.Clamp((int)(v * 255f), 0, 255);

            var r = Clamp(c.R);
            var g = Clamp(c.G);
            var b = Clamp(c.B);
            var a = Clamp(c.A);

            return (a << 24) | (r << 16) | (g << 8) | b;
        }       
    }
}
