using System.Runtime.InteropServices;

namespace AstraEngine.Platform.Windows;

internal static class Win32Native
{
    public const int WM_CLOSE = 0x0010;
    public const int WM_DESTROY = 0x0002;
    public const int WM_SIZE = 0x0005;
    public const int WM_QUIT = 0x0012;
    public const int WM_MOUSEMOVE = 0x0200;

    public const int WS_OVERLAPPEDWINDOW = 0x00CF0000;
    public const int WS_VISIBLE = 0x10000000;

    public const int CW_USEDEFAULT = unchecked((int)0x80000000);

    public const uint PM_REMOVE = 0x0001;
    public const uint SW_SHOW = 5;

    public const int GWLP_USERDATA = -21;

    public const int CS_HREDRAW = 0x0002;
    public const int CS_VREDRAW = 0x0001;

    public const uint SRCCOPY = 0x00CC0020;
    public const uint DIB_RGB_COLORS = 0;

    public const int PFD_DRAW_TO_WINDOW = 0x00000004;
    public const int PFD_SUPPORT_OPENGL = 0x00000020;
    public const int PFD_DOUBLEBUFFER = 0x00000001;
    public const byte PFD_TYPE_RGBA = 0;
    public const byte PFD_MAIN_PLANE = 0;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct WNDCLASSEX
    {
        public uint cbSize;
        public uint style;
        public WndProc lpfnWndProc;
        public int cbClsExtra;
        public int cbWndExtra;
        public nint hInstance;
        public nint hIcon;
        public nint hCursor;
        public nint hbrBackground;
        public string? lpszMenuName;
        public string lpszClassName;
        public nint hIconSm;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MSG
    {
        public nint hwnd;
        public uint message;
        public nint wParam;
        public nint lParam;
        public uint time;
        public Point pt;
        public uint lPrivate;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Point
    {
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BITMAPINFOHEADER
    {
        public uint biSize;
        public int biWidth;
        public int biHeight;
        public ushort biPlanes;
        public ushort biBitCount;
        public uint biCompression;
        public uint biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public uint biClrUsed;
        public uint biClrImportant;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BITMAPINFO
    {
        public BITMAPINFOHEADER bmiHeader;
        public uint bmiColors;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct PIXELFORMATDESCRIPTOR
    {
        public ushort nSize;
        public ushort nVersion;
        public int dwFlags;
        public byte iPixelType;
        public byte cColorBits;
        public byte cRedBits;
        public byte cRedShift;
        public byte cGreenBits;
        public byte cGreenShift;
        public byte cBlueBits;
        public byte cBlueShift;
        public byte cAlphaBits;
        public byte cAlphaShift;
        public byte cAccumBits;
        public byte cAccumRedBits;
        public byte cAccumGreenBits;
        public byte cAccumBlueBits;
        public byte cAccumAlphaBits;
        public byte cDepthBits;
        public byte cStencilBits;
        public byte cAuxBuffers;
        public byte iLayerType;
        public byte bReserved;
        public int dwLayerMask;
        public int dwVisibleMask;
        public int dwDamageMask;
    }

    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    internal delegate nint WndProc(nint hWnd, uint msg, nint wParam, nint lParam);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern ushort RegisterClassEx(ref WNDCLASSEX lpwcx);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern nint CreateWindowEx(
        int dwExStyle,
        string lpClassName,
        string lpWindowName,
        int dwStyle,
        int x,
        int y,
        int nWidth,
        int nHeight,
        nint hWndParent,
        nint hMenu,
        nint hInstance,
        nint lpParam);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool ShowWindow(nint hWnd, uint nCmdShow);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool UpdateWindow(nint hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool DestroyWindow(nint hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool PeekMessage(out MSG lpMsg, nint hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool TranslateMessage([In] ref MSG lpMsg);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern nint DispatchMessage([In] ref MSG lpMsg);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern bool SetWindowText(nint hWnd, string lpString);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool GetClientRect(nint hWnd, out RECT lpRect);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern nint GetModuleHandle(string? lpModuleName);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern nint DefWindowProc(nint hWnd, uint msg, nint wParam, nint lParam);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern nint SetWindowLongPtr(nint hWnd, int nIndex, nint dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern nint GetWindowLongPtr(nint hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern nint GetDC(nint hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern int ReleaseDC(nint hWnd, nint hDC);

    [DllImport("gdi32.dll", SetLastError = true)]
    internal static extern int StretchDIBits(
        nint hdc,
        int xDest,
        int yDest,
        int DestWidth,
        int DestHeight,
        int xSrc,
        int ySrc,
        int SrcWidth,
        int SrcHeight,
        nint lpBits,
        ref BITMAPINFO lpbmi,
        uint iUsage,
        uint rop);

    [DllImport("gdi32.dll", SetLastError = true)]
    internal static extern int ChoosePixelFormat(nint hdc, ref PIXELFORMATDESCRIPTOR ppfd);

    [DllImport("gdi32.dll", SetLastError = true)]
    internal static extern bool SetPixelFormat(nint hdc, int iPixelFormat, ref PIXELFORMATDESCRIPTOR ppfd);

    [DllImport("gdi32.dll", SetLastError = true)]
    internal static extern nint SwapBuffers(nint hdc);

    [DllImport("opengl32.dll", SetLastError = true)]
    internal static extern nint wglCreateContext(nint hdc);

    [DllImport("opengl32.dll", SetLastError = true)]
    internal static extern bool wglMakeCurrent(nint hdc, nint hglrc);

    [DllImport("opengl32.dll", SetLastError = true)]
    internal static extern bool wglDeleteContext(nint hglrc);
}