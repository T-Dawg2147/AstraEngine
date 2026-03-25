using System.Runtime.InteropServices;

namespace AstraEngine.Platform.Windows;

public sealed class WindowsWindow : IWindow
{
    private static readonly string WindowClassName = "AstraEngineWindowClass";
    private static readonly Win32Native.WndProc WindowProcedureDelegate = WindowProcedure;

    private bool _isOpen = true;
    private readonly nint _hWnd;
    private readonly GCHandle _selfHandle;

    private nint _hdc;
    private nint _hglrc;

    private int _lastMouseX;
    private int _lastMouseY;
    private bool _hasMoved;

    static WindowsWindow()
    {
        RegisterWindowClass();
    }

    public WindowsWindow(string title, int width, int height)
    {
        Title = title;
        Width = width;
        Height = height;
        State = WindowState.Normal;

        var hInstance = Win32Native.GetModuleHandle(null);

        _hWnd = Win32Native.CreateWindowEx(
            0,
            WindowClassName,
            title,
            Win32Native.WS_OVERLAPPEDWINDOW | Win32Native.WS_VISIBLE,
            Win32Native.CW_USEDEFAULT,
            Win32Native.CW_USEDEFAULT,
            width,
            height,
            0,
            0,
            hInstance,
            0);

        if (_hWnd == 0)
            throw new InvalidOperationException("Failed to create native window.");

        _selfHandle = GCHandle.Alloc(this);
        Win32Native.SetWindowLongPtr(_hWnd, Win32Native.GWLP_USERDATA, GCHandle.ToIntPtr(_selfHandle));
        Win32Native.ShowWindow(_hWnd, Win32Native.SW_SHOW);
        Win32Native.UpdateWindow(_hWnd);
    }

    public nint WindowHandle => _hWnd;

    public string Title { get; set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public WindowState State { get; private set; }
    public bool IsOpen => _isOpen;

    public event Action<WindowResizeEvent>? Resized;
    public event Action<WindowCloseEvent>? Closing;
    public event Action<float, float>? MouseMoved;

    public void InitializeOpenGl()
    {
        if (_hdc != 0)
            return;

        _hdc = Win32Native.GetDC(_hWnd);
        if (_hdc == 0)
            throw new InvalidOperationException("Failed to acquire device context.");

        var pfd = new Win32Native.PIXELFORMATDESCRIPTOR
        {
            nSize = (ushort)Marshal.SizeOf<Win32Native.PIXELFORMATDESCRIPTOR>(),
            nVersion = 1,
            dwFlags = Win32Native.PFD_DRAW_TO_WINDOW | Win32Native.PFD_SUPPORT_OPENGL | Win32Native.PFD_DOUBLEBUFFER,
            iPixelType = Win32Native.PFD_TYPE_RGBA,
            cColorBits = 32,
            cDepthBits = 24,
            cStencilBits = 8,
            iLayerType = Win32Native.PFD_MAIN_PLANE
        };

        var pixelFormat = Win32Native.ChoosePixelFormat(_hdc, ref pfd);
        if (pixelFormat == 0 || !Win32Native.SetPixelFormat(_hdc, pixelFormat, ref pfd))
            throw new InvalidOperationException("Failed to set pixel format.");

        _hglrc = Win32Native.wglCreateContext(_hdc);
        if (_hglrc == 0 || !Win32Native.wglMakeCurrent(_hdc, _hglrc))
            throw new InvalidOperationException("Failed to create OpenGL context.");
    }

    public void PollEvents()
    {
        while (Win32Native.PeekMessage(out var msg, 0, 0, 0, Win32Native.PM_REMOVE))
        {
            Win32Native.TranslateMessage(ref msg);
            Win32Native.DispatchMessage(ref msg);
        }
    }

    public void SetTitle(string title)
    {
        Title = title;
        Win32Native.SetWindowText(_hWnd, title);
    }

    public void Present(ReadOnlySpan<int> pixels, int width, int height)
    {
        if (_hWnd == 0 || pixels.IsEmpty)
            return;

        var hdc = Win32Native.GetDC(_hWnd);
        if (hdc == 0)
            return;

        try
        {
            var bmi = new Win32Native.BITMAPINFO
            {
                bmiHeader = new Win32Native.BITMAPINFOHEADER
                {
                    biSize = (uint)Marshal.SizeOf<Win32Native.BITMAPINFOHEADER>(),
                    biWidth = width,
                    biHeight = -height,
                    biPlanes = 1,
                    biBitCount = 32,
                    biCompression = 0,
                    biSizeImage = (uint)(width * height * 4)
                }
            };

            unsafe
            {
                fixed (int* pPixels = pixels)
                {
                    Win32Native.StretchDIBits(
                        hdc,
                        0,
                        0,
                        width,
                        height,
                        0,
                        0,
                        width,
                        height,
                        (nint)pPixels,
                        ref bmi,
                        Win32Native.DIB_RGB_COLORS,
                        Win32Native.SRCCOPY);
                }
            }
        }
        finally
        {
            if (_hdc == 0)
                Win32Native.ReleaseDC(_hWnd, hdc);
        }
    }

    public void SwapOpenGlBuffers()
    {
        if (_hdc != 0)
            Win32Native.SwapBuffers(_hdc);
    }

    public void Close()
    {
        if (!_isOpen)
        {
            return;
        }

        Closing?.Invoke(new WindowCloseEvent());
        _isOpen = false;
        Win32Native.DestroyWindow(_hWnd);
    }

    public void Dispose()
    {
        Close();

        if (_hglrc != 0)
        {
            Win32Native.wglMakeCurrent(0, 0);
            Win32Native.wglDeleteContext(_hglrc);
            _hglrc = 0;
        }

        if (_hdc != 0)
        {
            Win32Native.ReleaseDC(_hWnd, _hdc);
            _hdc = 0;
        }

        if (_selfHandle.IsAllocated)
            _selfHandle.Free();
    }

    private static void RegisterWindowClass()
    {
        var hInstance = Win32Native.GetModuleHandle(null);

        var wndClass = new Win32Native.WNDCLASSEX
        {
            cbSize = (uint)Marshal.SizeOf<Win32Native.WNDCLASSEX>(),
            style = (uint)(Win32Native.CS_HREDRAW | Win32Native.CS_VREDRAW),
            lpfnWndProc = WindowProcedureDelegate,
            cbClsExtra = 0,
            cbWndExtra = 0,
            hInstance = hInstance,
            hIcon = 0,
            hCursor = 0,
            hbrBackground = 0,
            lpszMenuName = null,
            lpszClassName = WindowClassName,
            hIconSm = 0
        };

        var atom = Win32Native.RegisterClassEx(ref wndClass);
        if (atom == 0)
        {
            var error = Marshal.GetLastWin32Error();
            if (error != 1410)
            {
                throw new InvalidOperationException($"Failed to register window class. Win32 error: {error}");
            }
        }
    }

    private static nint WindowProcedure(nint hWnd, uint msg, nint wParam, nint lParam)
    {
        var userData = Win32Native.GetWindowLongPtr(hWnd, Win32Native.GWLP_USERDATA);

        if (userData != 0)
        {
            var handle = GCHandle.FromIntPtr(userData);
            if (handle.Target is WindowsWindow window)
            {
                return window.HandleMessage(hWnd, msg, wParam, lParam);
            }
        }

        return Win32Native.DefWindowProc(hWnd, msg, wParam, lParam);
    }

    private nint HandleMessage(nint hWnd, uint msg, nint wParam, nint lParam)
    {
        switch (msg)
        {
            case Win32Native.WM_CLOSE:
                Closing?.Invoke(new WindowCloseEvent());
                _isOpen = false;
                Win32Native.DestroyWindow(hWnd);
                return 0;

            case Win32Native.WM_DESTROY:
                _isOpen = false;
                return 0;

            case Win32Native.WM_SIZE:
                if (Win32Native.GetClientRect(hWnd, out var rect))
                {
                    Width = rect.Right - rect.Left;
                    Height = rect.Bottom - rect.Top;
                    Resized?.Invoke(new WindowResizeEvent(Width, Height));
                }
                return 0;

            case Win32Native.WM_MOUSEMOVE:
                {
                    var x = unchecked((short)(lParam.ToInt64() & 0xFFFF));
                    var y = unchecked((short)((lParam.ToInt64() >> 16) & 0xFFFF));

                    if (_hasMoved)
                        MouseMoved?.Invoke(x - _lastMouseX, y - _lastMouseY);

                    _lastMouseX = x;
                    _lastMouseY = y;
                    _hasMoved = true;
                    return 0;
                }                

            default:
                return Win32Native.DefWindowProc(hWnd, msg, wParam, lParam);
        }
    }
}