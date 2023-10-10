using System.Numerics;
using RedHerring.Fingerprint.Devices;

namespace RedHerring.Fingerprint;

public interface IInput
{
    void Tick();
    void EnableDebug();
    void DisableDebug();
    
    IKeyboardState? Keyboard { get; }
    IMouseState? Mouse { get; }
    IGamepadState? Gamepad { get; }
    
    Vector2 MousePosition { get; }
    Vector2 MouseDelta { get; }
    float MouseWheelDelta { get; }
    
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
}