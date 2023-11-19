using System.Numerics;

namespace RedHerring.Fingerprint;

public partial class SilkInput
{
    #region Queries

    public bool AreModifiersDown(Modifiers modifiers)
    {
        if (modifiers == Modifiers.None)
        {
            return true;
        }
        
        return _keyboardState is not null && (_keyboardState.Modifiers & modifiers) != 0;
    }

    public bool IsKeyUp(Key key) => _keyboardState?.IsKeyUp(key) ?? true;
    public bool IsKeyPressed(Key key) => _keyboardState?.IsKeyPressed(key) ?? false;
    public bool IsKeyDown(Key key) => _keyboardState?.IsKeyDown(key) ?? false;
    public bool IsKeyReleased(Key key) => _keyboardState?.IsKeyReleased(key) ?? false;
    public bool IsAnyKeyDown() => _keyboardState?.IsAnyKeyDown() ?? false;
    public void KeysDown(IList<Key> keys) => _keyboardState?.KeysDown(keys);
    public bool IsButtonUp(MouseButton button) => _mouseState?.IsButtonUp(button) ?? true;
    public bool IsButtonPressed(MouseButton button) => _mouseState?.IsButtonPressed(button) ?? false;
    public bool IsButtonDown(MouseButton button) => _mouseState?.IsButtonDown(button) ?? false;
    public bool IsButtonReleased(MouseButton button) => _mouseState?.IsButtonReleased(button) ?? false;
    public void ButtonsDown(IList<MouseButton> buttons) => _mouseState?.ButtonsDown(buttons);
    public bool IsAnyMouseButtonDown() => _mouseState?.IsAnyButtonDown() ?? false;
    public bool IsMouseMoved(MouseAxis axis) => _mouseState?.IsMoved(axis) ?? false;
    public float Axis(MouseAxis axis) => _mouseState?.Axis(axis) ?? 0;
    public Vector2 MousePosition => _mouseState?.Position ?? Vector2.Zero;
    public Vector2 MouseDelta => _mouseState?.Delta ?? Vector2.Zero;
    public float MouseWheelDelta => _mouseState?.ScrollWheel.Y ?? 0;
    public bool IsButtonUp(GamepadButton button) => _gamepadState?.IsButtonUp(button) ?? true;
    public bool IsButtonPressed(GamepadButton button) => _gamepadState?.IsButtonPressed(button) ?? false;
    public bool IsButtonDown(GamepadButton button) => _gamepadState?.IsButtonDown(button) ?? false;
    public bool IsButtonReleased(GamepadButton button) => _gamepadState?.IsButtonReleased(button) ?? false;
    public bool IsAnyGamepadButtonDown() => _gamepadState?.IsAnyButtonDown() ?? false;
    public void ButtonsDown(IList<GamepadButton> buttons) => _gamepadState?.ButtonsDown(buttons);
    public float Axis(GamepadAxis axis) => _gamepadState?.Axis(axis) ?? 0;
    public bool IsActionUp(string action) => _actionsState.IsActionUp(action);
    public bool IsActionPressed(string action) => _actionsState.IsActionPressed(action);
    public bool IsActionDown(string action) => _actionsState.IsActionDown(action);
    public bool IsActionReleased(string action) => _actionsState.IsActionReleased(action);
    public bool IsAnyActionDown() => _actionsState.IsAnyActionDown();
    public void ActionsDown(IList<string> actions) => _actionsState.ActionsDown(actions);

    #endregion Queries
}