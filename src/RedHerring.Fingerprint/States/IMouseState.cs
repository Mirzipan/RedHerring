using System.Numerics;

namespace RedHerring.Fingerprint.States;

public interface IMouseState : IInputState
{
    bool IsButtonUp(MouseButton button);
    bool IsButtonPressed(MouseButton button);
    bool IsButtonDown(MouseButton button);
    bool IsButtonReleased(MouseButton button);
    bool IsMoved(MouseAxis axis);
    Vector2 Position { get; }
    Vector2 Delta { get; }
    Vector2 ScrollWheel { get; }
    void GetButtonsDown(IList<MouseButton> buttons);
}