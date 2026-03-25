namespace AstraEngine.Input
{
    public enum KeyCode
    {
        Unknown,
        W, A, S, D,
        Q, E,
        Up, Down, Left, Right,
        Space, Escape,
        LeftShift, RightShift,
        LeftControl, RightControl,
        MouseLeft, MouseRight, MouseMiddle
    }

    public readonly struct KeyEvent
    {
        public KeyEvent(KeyCode key, bool isPressed)
        {
            Key = key;
            IsPressed = isPressed;
        }

        public KeyCode Key { get; }
        public bool IsPressed { get; }
    }

    public readonly struct MouseMoveEvent
    {
        public MouseMoveEvent(float deltaX, float deltaY)
        {
            DeltaX = deltaX;
            DeltaY = deltaY;
        }

        public float DeltaX { get; }
        public float DeltaY { get; }
    }

    public readonly struct MouseButtonEvent
    {
        public MouseButtonEvent(KeyCode button, bool isPressed)
        {
            Button = button;
            IsPressed = isPressed;
        }

        public KeyCode Button { get; }
        public bool IsPressed { get; }
    }

    public readonly struct MouseScrollEvent
    {
        public MouseScrollEvent(float delta)
        {
            Delta = delta;
        }

        public float Delta { get; }
    }
}
