using System.Numerics;
using RedHerring.Alexandria.Masks;
using Silk.NET.Input;
using SilkButton = Silk.NET.Input.ButtonName;

namespace RedHerring.Fingerprint.Devices;

public class GamepadState : IGamepadState, IInputSource, IDisposable
{
    internal static Action<string>? DebugPrint;
    
    private BitMask16 _down = new BitMask16(false);
    private BitMask16 _pressed = new BitMask16(false);
    private BitMask16 _released = new BitMask16(false);
    
    private IGamepad _gamepad;

    private Vector2 _leftThumb;
    private Vector2 _rightThumb;

    private float _leftTrigger;
    private float _rightTrigger;
    
    public string Name => _gamepad.Name;
    public IInputDevice Device => _gamepad;

    public Vector2 LeftThumb => _leftThumb;
    public Vector2 RightThumb => _rightThumb;
    public float LeftTrigger => _leftTrigger;
    public float RightTrigger => _rightTrigger;

    public int Priority { get; set; }

    #region Lifecycle

    public GamepadState(IGamepad gamepad)
    {
        _gamepad = gamepad;
        
        _gamepad.ButtonDown += OnButtonDown;
        _gamepad.ButtonUp += OnButtonUp;
        _gamepad.ThumbstickMoved += OnThumbstickMoved;
        _gamepad.TriggerMoved += OnTriggerMoved;
    }

    public void Reset()
    {
        _pressed.Set(false);
        _released.Set(false);
    }

    public void Dispose()
    {
        _gamepad.ButtonDown -= OnButtonDown;
        _gamepad.ButtonUp -= OnButtonUp;
        _gamepad.ThumbstickMoved -= OnThumbstickMoved;
        _gamepad.TriggerMoved -= OnTriggerMoved;
        
        _gamepad = null!;
    }

    #endregion Lifecycle

    #region Queries

    public bool IsButtonUp(GamepadButton button) => !_down.Get((int)button);
    public bool IsButtonPressed(GamepadButton button) => _pressed.Get((int)button);
    public bool IsButtonDown(GamepadButton button) => _down.Get((int)button);
    public bool IsButtonReleased(GamepadButton button) => _released.Get((int)button);
    public bool IsAnyButtonDown() => _down != BitMask16.Empty;
    public void GetButtonsDown(IList<GamepadButton> buttons)
    {
        for (int i = 0; i < BitMask16.Count; i++)
        {
            if (_down[i])
            {
                buttons.Add((GamepadButton)i);
            }
        }
    }
    
    public float GetAxis(GamepadAxis axis)
    {
        return axis switch
        {
            GamepadAxis.None => 0,
            GamepadAxis.LeftX => _leftThumb.X,
            GamepadAxis.LeftY => _leftThumb.Y,
            GamepadAxis.RightX => _rightThumb.X,
            GamepadAxis.RightY => _rightThumb.Y,
            GamepadAxis.TriggerLeft => _leftTrigger,
            GamepadAxis.TriggerRight => _rightTrigger,
            _ => 0,
        };
    }

    #endregion Queries
    
    #region Private

    private static GamepadButton ConvertButton(SilkButton button)
    {
        return button switch
        {
            SilkButton.Unknown => GamepadButton.Unknown,
            SilkButton.A => GamepadButton.A,
            SilkButton.B => GamepadButton.B,
            SilkButton.X => GamepadButton.X,
            SilkButton.Y => GamepadButton.Y,
            SilkButton.LeftBumper => GamepadButton.LeftBumper,
            SilkButton.RightBumper => GamepadButton.RightBumper,
            SilkButton.Back => GamepadButton.Back,
            SilkButton.Start => GamepadButton.Start,
            SilkButton.Home => GamepadButton.Home,
            SilkButton.LeftStick => GamepadButton.LeftStick,
            SilkButton.RightStick => GamepadButton.RightStick,
            SilkButton.DPadUp => GamepadButton.DPadUp,
            SilkButton.DPadRight => GamepadButton.DPadRight,
            SilkButton.DPadDown => GamepadButton.DPadDown,
            SilkButton.DPadLeft => GamepadButton.DPadLeft,
            _ => GamepadButton.Unknown,
        };
    }

    #endregion Private

    #region Bindings

    private void OnButtonDown(IGamepad gamepad, Button buttonState)
    {
        var button = ConvertButton(buttonState.Name);
        _pressed[(int)button] = true;
        _down[(int)button] = true;
        
        DebugPrint?.Invoke($"`{gamepad}` button `{button}` pressed.");
    }

    private void OnButtonUp(IGamepad gamepad, Button buttonState)
    {
        var button = ConvertButton(buttonState.Name);
        _pressed[(int)button] = true;
        _down[(int)button] = true;
        
        DebugPrint?.Invoke($"`{gamepad}` button `{button}` released.");
    }

    private void OnThumbstickMoved(IGamepad gamepad, Thumbstick thumbstick)
    {
        string thumbName = $"thumbstick {thumbstick.Index}";
        switch (thumbstick.Index)
        {
            case 0:
                thumbName = "Left thumbstick";
                _leftThumb.X = thumbstick.X;
                _leftThumb.Y = thumbstick.Y;
                break;
            case 1:
                thumbName = "Right thumbstick";
                _rightThumb.X = thumbstick.X;
                _rightThumb.Y = thumbstick.Y;
                break;
        }

        DebugPrint?.Invoke($"`{gamepad}` `{thumbName}` moved to `<{thumbstick.X} {thumbstick.Y}>`.");
    }

    private void OnTriggerMoved(IGamepad gamepad, Trigger trigger)
    {
        var axis = GamepadAxis.None;
        switch (trigger.Index)
        {
            case 0:
                axis = GamepadAxis.TriggerLeft;
                _leftTrigger = trigger.Position;
                break;
            case 1:
                axis = GamepadAxis.TriggerRight;
                _rightTrigger = trigger.Position;
                break;
        }

        DebugPrint?.Invoke($"`{gamepad}` `{axis}` moved to `{trigger.Position}`.");
    }

    #endregion Bindings
}