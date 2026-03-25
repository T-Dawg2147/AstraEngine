using AstraEngine.Graphics.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstraEngine.Graphics.OpenGL
{
    public sealed class OpenGLTexture : ITexture
    {
        public OpenGLTexture(TextureDescription description)
        {
            Description = description;
        }

        public TextureDescription Description { get; }

        public int Width => Description.Width;
        public int Height => Description.Height;
        public PixelFormat Format => Description.Format;

        public void Dispose() { }
    }
}
