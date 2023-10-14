using System.Numerics;
using RedHerring.Fingerprint.Layers;
using RedHerring.Fingerprint.Shortcuts;
using RedHerring.Fingerprint.States;

namespace RedHerring.Fingerprint;

public interface IInput
{
    void Tick();
    void EnableDebug();
    void DisableDebug();
    
    IKeyboardState? Keyboard { get; }
    IMouseState? Mouse { get; }
    IGamepadState? Gamepad { get; }
    IActionState Actions { get; }
    
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
    void GetKeysDown(IList<Key> keys);
    
    bool IsButtonUp(MouseButton button);
    bool IsButtonPressed(MouseButton button);
    bool IsButtonDown(MouseButton button);
    bool IsButtonReleased(MouseButton button);
    bool IsAnyMouseButtonDown();
    void GetButtonsDown(IList<MouseButton> buttons);
    bool IsMouseMoved(MouseAxis axis);
    float GetAxis(MouseAxis axis);
    
    bool IsButtonUp(GamepadButton button);
    bool IsButtonPressed(GamepadButton button);
    bool IsButtonDown(GamepadButton button);
    bool IsButtonReleased(GamepadButton button);
    bool IsAnyGamepadButtonDown();
    void GetButtonsDown(IList<GamepadButton> buttons);
    float GetAxis(GamepadAxis axis);

    bool IsActionUp(string action);
    bool IsActionPressed(string action);
    bool IsActionDown(string action);
    bool IsActionReleased(string action);
    bool IsAnyActionDown();
    void GetActionsDown(IList<string> actions);
}