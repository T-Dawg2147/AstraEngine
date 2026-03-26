namespace AstraEngine.Input;

public sealed class InputState
{
    private readonly HashSet<KeyCode> _keysDown = [];
    private readonly HashSet<KeyCode> _keysPressed = [];
    private readonly HashSet<KeyCode> _keysReleased = [];

    public float MouseX { get; internal set; }
    public float MouseY { get; internal set; }
    public float MouseDeltaX { get; internal set; }
    public float MouseDeltaY { get; internal set; }
    public float ScrollDelta { get; internal set; }

    public bool IsKeyDown(KeyCode key) => _keysDown.Contains(key);
    public bool WasKeyPressed(KeyCode key) => _keysPressed.Contains(key);
    public bool WasKeyReleased(KeyCode key) => _keysReleased.Contains(key);

    internal void BeginFrame()
    {
        _keysPressed.Clear();
        _keysReleased.Clear();
        MouseDeltaX = 0f;
        MouseDeltaY = 0f;
        ScrollDelta = 0f;
    }

    internal void SetKey(KeyCode key, bool down)
    {
        if (down)
        {
            if (_keysDown.Add(key))
            {
                _keysPressed.Add(key);
            }
        }
        else
        {
            if (_keysDown.Remove(key))
            {
                _keysReleased.Add(key);
            }
        }
    }

    internal void AddMouseDelta(float dx, float dy)
    {
        MouseDeltaX += dx;
        MouseDeltaY += dy;
    }

    internal void SetMousePosition(float x, float y)
    {
        MouseX = x;
        MouseY = y;
    }

    internal void AddScrollDelta(float delta)
    {
        ScrollDelta += delta;
    }
}