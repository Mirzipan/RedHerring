using System.Numerics;
using RedHerring.Fingerprint.Events;
using RedHerring.Fingerprint.Layers;
using RedHerring.Fingerprint.Shortcuts;
using RedHerring.Fingerprint.States;

namespace RedHerring.Fingerprint;

public interface Input
{
    void Tick();
    bool IsDebugging { get; }
    void EnableDebug();
    void DisableDebug();
    
    KeyboardState? Keyboard { get; }
    MouseState? Mouse { get; }
    GamepadState? Gamepad { get; }
    ActionsState Actions { get; }
    
    Vector2 MousePosition { get; }
    Vector2 MouseDelta { get; }
    float MouseWheelDelta { get; }
    ShortcutBindings? Bindings { get; set; }
    InputLayers Layers { get; }

    bool IsKeyUp(Key key);
    bool IsKeyPressed(Key key);
    bool IsKeyDown(Key key);
    bool IsKeyReleased(Key key);
    bool IsAnyKeyDown();
    void KeysDown(IList<Key> keys);
    
    bool IsButtonUp(MouseButton button);
    bool IsButtonPressed(MouseButton button);
    bool IsButtonDown(MouseButton button);
    bool IsButtonReleased(MouseButton button);
    bool IsAnyMouseButtonDown();
    void ButtonsDown(IList<MouseButton> buttons);
    bool IsMouseMoved(MouseAxis axis);
    float Axis(MouseAxis axis);
    
    bool IsButtonUp(GamepadButton button);
    bool IsButtonPressed(GamepadButton button);
    bool IsButtonDown(GamepadButton button);
    bool IsButtonReleased(GamepadButton button);
    bool IsAnyGamepadButtonDown();
    void ButtonsDown(IList<GamepadButton> buttons);
    float Axis(GamepadAxis axis);

    bool IsActionUp(string action);
    bool IsActionPressed(string action);
    bool IsActionDown(string action);
    bool IsActionReleased(string action);
    bool IsAnyActionDown();
    void ActionsDown(IList<string> actions);
}