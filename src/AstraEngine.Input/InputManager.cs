namespace AstraEngine.Input;

public sealed class InputManager
{
    public InputManager()
    {
        Current = new InputState();
        Previous = new InputState();
    }

    public InputState Current { get; }
    public InputState Previous { get; }

    public void BeginFrame()
    {
        Current.BeginFrame();
    }

    public void EndFrame()
    {
        // Reserved for future processing.
    }

    public void HandleKeyEvent(in KeyEvent keyEvent)
    {
        Current.SetKey(keyEvent.Key, keyEvent.IsPressed);
    }

    public void HandleMouseMove(in MouseMoveEvent mouseEvent)
    {
        Current.AddMouseDelta(mouseEvent.DeltaX, mouseEvent.DeltaY);
    }

    public void HandleMouseButton(in MouseButtonEvent mouseEvent)
    {
        Current.SetKey(mouseEvent.Button, mouseEvent.IsPressed);
    }

    public void HandleMouseScroll(in MouseScrollEvent scrollEvent)
    {
        // Reserved for future scroll state.
    }
}