namespace AstraEngine.Graphics.OpenGL
{
    internal static class OpenGLConstants
    {
        public const uint GL_COLOR_BUFFER_BIT = 0x00004000;
        public const uint GL_DEPTH_BUFFER_BIT = 0x00000100;

        public const uint GL_TRIANGLES = 0x0004;
        public const uint GL_ARRAY_BUFFER = 0x8892;
        public const uint GL_ELEMENT_ARRAY_BUFFER = 0x8893;
        public const uint GL_STATIC_DRAW = 0x88E4;

        public const uint GL_FLOAT = 0x1406;
        public const uint GL_UNSIGNED_INT = 0x1405;

        public const uint GL_VERTEX_SHADER = 0x8B31;
        public const uint GL_FRAGMENT_SHADER = 0x8B30;

        public const uint GL_COMPILE_STATUS = 0x8B81;
        public const uint GL_LINK_STATUS = 0x8B82;

        public const uint GL_FALSE = 0;
        public const uint GL_TRUE = 1;

        // Depth testing
        public const uint GL_DEPTH_TEST = 0x0B71;
        public const uint GL_LEQUAL = 0x0203;

        // Face culling
        public const uint GL_CULL_FACE = 0x0B44;
        public const uint GL_BACK = 0x0405;
        public const uint GL_CW = 0x0900;
    }
}