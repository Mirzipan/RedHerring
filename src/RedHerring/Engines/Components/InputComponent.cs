using System.Numerics;
using RedHerring.Alexandria;
using RedHerring.Fingerprint;
using RedHerring.Fingerprint.Layers;
using RedHerring.Fingerprint.Shortcuts;
using RedHerring.Fingerprint.States;

namespace RedHerring.Engines.Components;

public class InputComponent : AnEngineComponent, IUpdatable
{
    private IInput _input = null!;

    public bool IsEnabled => true;
    public int UpdateOrder => -1_000_000;

    public IKeyboardState? Keyboard => _input.Keyboard;
    public IMouseState? Mouse => _input.Mouse;

    private InputReceiver _receiver = new InputReceiver("input_debug");

    #region Lifecycle

    protected override void Init()
    {
        _input = new Input(Context.View);
        
        AddDebugBindings();
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

    #region Debug

    private void AddDebugBindings()
    {
        if (_input.Bindings is null)
        {
            return;
        }
        
        var shortcut = new CompositeShortcut();
        //shortcut.Add(new KeyboardShortcut(Key.ShiftLeft));
        shortcut.Add(new KeyboardShortcut(Key.F12));
        _input.Bindings.Add(new ShortcutBinding("dbg_draw", shortcut));
        
        _receiver.Bind("dbg_draw", InputState.Released, ToggleDebugDraw);
        _input.Layers.Push(_receiver);
    }

    private void ToggleDebugDraw(ref ActionEvent evt)
    {
        evt.Consumed = true;
        
        if (_input.IsDebugging)
        {
            _input.DisableDebug();
        }
        else
        {
            _input.EnableDebug();
        }
    }

    #endregion Debug
}