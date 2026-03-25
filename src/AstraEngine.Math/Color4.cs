using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstraEngine.Math
{
    public readonly struct Color4
    {
        public Color4(float r, float g, float b, float a = 1f)
        {
            R = r; G = g; B = b; A = a;
        }

        public float R { get; }
        public float G { get; }
        public float B { get; }
        public float A { get; }

        public static Color4 Black => new(0f, 0f, 0f, 1f);
        public static Color4 White => new(1f, 1f, 1f, 1f);
        public static Color4 Transparent => new(0f, 0f, 0f, 0f);
    }
}
