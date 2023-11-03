using System.Numerics;
using RedHerring.Fingerprint.Layers;
using RedHerring.Fingerprint.Shortcuts;
using RedHerring.Fingerprint.States;

namespace RedHerring.Fingerprint;

public sealed class NullInput : Input
{
    private InputLayers _layers = new();
    private ActionsState _actions = new();
    
    private bool _isDebugging;
    
    public void Tick()
    {
    }

    public bool IsDebugging => _isDebugging;
    public void EnableDebug() => _isDebugging = true;
    public void DisableDebug() => _isDebugging = false;

    public KeyboardState? Keyboard => null;
    public MouseState? Mouse => null;
    public GamepadState? Gamepad => null;
    public ActionsState Actions => _actions;
    public Vector2 MousePosition => Vector2.Zero;
    public Vector2 MouseDelta => Vector2.Zero;
    public float MouseWheelDelta => 0f;
    public ShortcutBindings? Bindings { get; set; }
    public InputLayers Layers => _layers;
    
    public bool IsKeyUp(Key key) => true;
    public bool IsKeyPressed(Key key) => false;
    public bool IsKeyDown(Key key) => false;
    public bool IsKeyReleased(Key key) => false;
    public bool IsAnyKeyDown() => false;
    public void KeysDown(IList<Key> keys)
    {
    }
    
    public bool IsButtonUp(MouseButton button) => true;
    public bool IsButtonPressed(MouseButton button) => false;
    public bool IsButtonDown(MouseButton button) => false;
    public bool IsButtonReleased(MouseButton button) => false;
    public bool IsAnyMouseButtonDown() => false;
    public void ButtonsDown(IList<MouseButton> buttons)
    {
    }

    public bool IsMouseMoved(MouseAxis axis) => false;
    public float Axis(MouseAxis axis) => 0f;

    public bool IsButtonUp(GamepadButton button) => true;
    public bool IsButtonPressed(GamepadButton button) => false;
    public bool IsButtonDown(GamepadButton button) => false;
    public bool IsButtonReleased(GamepadButton button) => false;
    public bool IsAnyGamepadButtonDown() => false;
    public void ButtonsDown(IList<GamepadButton> buttons)
    {
    }

    public float Axis(GamepadAxis axis) => 0f;
    public bool IsActionUp(string action) => false;
    public bool IsActionPressed(string action) => false;
    public bool IsActionDown(string action) => false;
    public bool IsActionReleased(string action) => false;
    public bool IsAnyActionDown() => false;
    public void ActionsDown(IList<string> actions)
    {
    }
}