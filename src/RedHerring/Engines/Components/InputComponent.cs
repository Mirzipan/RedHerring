using System.Numerics;
using RedHerring.Alexandria;
using RedHerring.Fingerprint;
using RedHerring.Fingerprint.States;

namespace RedHerring.Engines.Components;

public class InputComponent : AnEngineComponent, IUpdatable
{
    private IInput _input = null!;

    public bool IsEnabled => true;
    public int UpdateOrder => -1_000_000;

    public IKeyboardState? Keyboard => _input.Keyboard;
    public IMouseState? Mouse => _input.Mouse;

    #region Lifecycle

    protected override void Init()
    {
        _input = new Input(Context.View);
        _input.EnableDebug();
    }
    public void Update(GameTime gameTime)
    {
        _input.Tick();
    }

    #endregion Lifecycle

    #region Queries

    public bool IsKeyUp(Key key) => _input.IsKeyUp(key);
    public bool IsKeyPressed(Key key) => _input.IsKeyPressed(key);
    public bool IsKeyDown(Key key) => _input.IsKeyDown(key);
    public bool IsKeyReleased(Key key) => _input.IsKeyReleased(key);
    public bool IsAnyKeyDown() => _input.IsAnyKeyDown();
    public void GetKeysDown(IList<Key> keys) => _input.GetKeysDown(keys);
    
    public bool IsButtonUp(MouseButton button) => _input.IsButtonUp(button);
    public bool IsButtonPressed(MouseButton button) => _input.IsButtonPressed(button);
    public bool IsButtonDown(MouseButton button) => _input.IsButtonDown(button);
    public bool IsButtonReleased(MouseButton button) => _input.IsButtonReleased(button);
    public bool IsAnyMouseButtonDown() => _input.IsAnyMouseButtonDown();
    public void GetButtonsDown(IList<MouseButton> buttons) => _input.GetButtonsDown(buttons);
    public bool IsMouseMoved(MouseAxis axis) => _input.IsMouseMoved(axis);
    public float GetAxis(MouseAxis axis) => _input.GetAxis(axis);
    public Vector2 MousePosition => _input.MousePosition;
    public Vector2 MouseDelta => _input.MouseDelta;
    public float MouseWheelDelta => _input.MouseWheelDelta;
    
    public bool IsButtonUp(GamepadButton button) => _input.IsButtonUp(button);
    public bool IsButtonPressed(GamepadButton button) => _input.IsButtonPressed(button);
    public bool IsButtonDown(GamepadButton button) => _input.IsButtonDown(button);
    public bool IsButtonReleased(GamepadButton button) => _input.IsButtonReleased(button);
    public bool IsAnyButtonDown() => _input.IsAnyGamepadButtonDown();
    public void GetButtonsDown(IList<GamepadButton> buttons) => _input.GetButtonsDown(buttons);
    public float GetAxis(GamepadAxis axis) => _input.GetAxis(axis);

    #endregion Queries
}