using System.Runtime.InteropServices;

namespace AstraEngine.Platform.Windows
{
    public static class OpenGLProcLoader
    {
        [DllImport("opengl32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern nint wglGetProcAddress(string lpszProc);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern nint GetProcAddress(nint hModule, string procName);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern nint GetModuleHandle(string? lpModuleName);

        public static nint Load(string name)
        {
            var proc = wglGetProcAddress(name);
            if (IsValidProc(proc))
                return proc;

            var openglModule = GetModuleHandle("opengl32.dll");
            if (openglModule != 0)
            {
                proc = GetProcAddress(openglModule, name);
                if (IsValidProc(proc))
                    return proc;
            }                
            return 0;
        }

        private static bool IsValidProc(nint proc)
            => proc != 0 && proc != (nint)1 && proc != (nint)2 && proc != (nint)3 && proc != (nint)(-1);
    }
}
