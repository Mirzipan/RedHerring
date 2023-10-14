using System.Numerics;

namespace RedHerring.Fingerprint;

public partial class Input
{
    #region Queries

    public bool IsKeyUp(Key key) => _keyboardState?.IsKeyUp(key) ?? true;
    public bool IsKeyPressed(Key key) => _keyboardState?.IsKeyPressed(key) ?? false;
    public bool IsKeyDown(Key key) => _keyboardState?.IsKeyDown(key) ?? false;
    public bool IsKeyReleased(Key key) => _keyboardState?.IsKeyReleased(key) ?? false;
    public bool IsAnyKeyDown() => _keyboardState?.IsAnyKeyDown() ?? false;
    public void GetKeysDown(IList<Key> keys) => _keyboardState?.GetKeysDown(keys);
    public bool IsButtonUp(MouseButton button) => _mouseState?.IsButtonUp(button) ?? true;
    public bool IsButtonPressed(MouseButton button) => _mouseState?.IsButtonPressed(button) ?? false;
    public bool IsButtonDown(MouseButton button) => _mouseState?.IsButtonDown(button) ?? false;
    public bool IsButtonReleased(MouseButton button) => _mouseState?.IsButtonReleased(button) ?? false;
    public void GetButtonsDown(IList<MouseButton> buttons) => _mouseState?.GetButtonsDown(buttons);
    public bool IsAnyMouseButtonDown() => _mouseState?.IsAnyButtonDown() ?? false;
    public bool IsMouseMoved(MouseAxis axis) => _mouseState?.IsMoved(axis) ?? false;
    public float GetAxis(MouseAxis axis) => _mouseState?.GetAxis(axis) ?? 0;
    public Vector2 MousePosition => _mouseState?.Position ?? Vector2.Zero;
    public Vector2 MouseDelta => _mouseState?.Delta ?? Vector2.Zero;
    public float MouseWheelDelta => _mouseState?.ScrollWheel.Y ?? 0;
    public bool IsButtonUp(GamepadButton button) => _gamepadState?.IsButtonUp(button) ?? true;
    public bool IsButtonPressed(GamepadButton button) => _gamepadState?.IsButtonPressed(button) ?? false;
    public bool IsButtonDown(GamepadButton button) => _gamepadState?.IsButtonDown(button) ?? false;
    public bool IsButtonReleased(GamepadButton button) => _gamepadState?.IsButtonReleased(button) ?? false;
    public bool IsAnyGamepadButtonDown() => _gamepadState?.IsAnyButtonDown() ?? false;
    public void GetButtonsDown(IList<GamepadButton> buttons) => _gamepadState?.GetButtonsDown(buttons);
    public float GetAxis(GamepadAxis axis) => _gamepadState?.GetAxis(axis) ?? 0;
    public bool IsActionUp(string action) => _actionsState.IsActionUp(action);
    public bool IsActionPressed(string action) => _actionsState.IsActionPressed(action);
    public bool IsActionDown(string action) => _actionsState.IsActionDown(action);
    public bool IsActionReleased(string action) => _actionsState.IsActionReleased(action);
    public bool IsAnyActionDown() => _actionsState.IsAnyActionDown();
    public void GetActionsDown(IList<string> actions) => _actionsState.GetActionsDown(actions);

    #endregion Queries
}