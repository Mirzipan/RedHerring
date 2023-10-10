using System.Numerics;
using RedHerring.Fingerprint.Devices;
using Silk.NET.Input;
using Silk.NET.Input.Sdl;
using Silk.NET.Windowing;

namespace RedHerring.Fingerprint;

public class Input: IInput, IDisposable
{
    private IInputContext _inputContext;

    private KeyboardState? _keyboardState;
    private MouseState? _mouseState;
    private IGamepad? _activeGamepad;

    private ControllerState _lastControllerState;
    private ControllerState _currentControllerState;
    
    private bool _isDebugging;

    public IKeyboardState? Keyboard => _keyboardState;
    public IMouseState? Mouse => _mouseState;
    
    #region Lifecycle

    public Input(IView view)
    {
        SdlInput.RegisterPlatform();

        _inputContext = view.CreateInput();
        _inputContext.ConnectionChanged += OnConnectionChanged;

        KeyboardState.DebugPrint = DebugPrint;
        MouseState.DebugPrint = DebugPrint;
        
        FindKeyboard(null);
        FindMouse(null);
        FindGamepad(null);
    }

    public void Tick()
    {
        _keyboardState?.Update();
        _mouseState?.Update();
        _lastControllerState = _currentControllerState;
    }

    public void Dispose()
    {
        _inputContext.ConnectionChanged -= OnConnectionChanged;
        _inputContext.Dispose();
    }

    #endregion Lifecycle

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
    public bool IsAnyMouseButtonDown() => _mouseState?.IsAnyButtonDown() ?? false;
    public bool IsMouseMoved(MouseAxis axis) => _mouseState?.IsMoved(axis) ?? false;
    public void GetButtonsDown(IList<MouseButton> buttons) => _mouseState?.GetButtonsDown(buttons);

    public Vector2 MousePosition => _mouseState?.Position ?? Vector2.Zero;
    public Vector2 MouseDelta => _mouseState?.Delta ?? Vector2.Zero;
    public float MouseWheelDelta => _mouseState?.ScrollWheel.Y ?? 0;

    public bool IsButtonUp(ButtonName button) => !_activeGamepad?.Buttons.FirstOrDefault(e => e.Name == button).Pressed ?? true;
    public bool IsButtonDown(ButtonName button) => _activeGamepad?.Buttons.FirstOrDefault(e => e.Name == button).Pressed ?? false;

    public bool IsUp(InputValue input)
    {
        return input.Source switch
        {
            InputSource.Keyboard => IsKeyUp(input.GetKey()),
            InputSource.MouseButton => IsButtonUp(input.GetMouseButton()),
            InputSource.MouseAxis => IsMouseMoved(input.GetMouseAxis()),
            InputSource.ControllerButton => IsButtonUp(input.GetControllerButton()),
            InputSource.ControllerAnalogButton => true,
            InputSource.ControllerAxis => true,
            _ => true,
        };
    }

    public bool IsPressed(InputValue input)
    {
        return input.Source switch
        {
            InputSource.Keyboard => IsKeyPressed(input.GetKey()),
            InputSource.MouseButton => IsButtonPressed(input.GetMouseButton()),
            InputSource.MouseAxis => IsMouseMoved(input.GetMouseAxis()),
            InputSource.ControllerButton => false,
            InputSource.ControllerAnalogButton => false,
            InputSource.ControllerAxis => false,
            _ => true,
        };
    }

    public bool IsDown(InputValue input)
    {
        return input.Source switch
        {
            InputSource.Keyboard => IsKeyDown(input.GetKey()),
            InputSource.MouseButton => IsButtonDown(input.GetMouseButton()),
            InputSource.MouseAxis => IsMouseMoved(input.GetMouseAxis()),
            InputSource.ControllerButton => IsButtonDown(input.GetControllerButton()),
            InputSource.ControllerAnalogButton => false,
            InputSource.ControllerAxis => false,
            _ => true,
        };
    }

    public bool IsReleased(InputValue input)
    {
        return input.Source switch
        {
            InputSource.Keyboard => IsKeyReleased(input.GetKey()),
            InputSource.MouseButton => IsButtonReleased(input.GetMouseButton()),
            InputSource.MouseAxis => IsMouseMoved(input.GetMouseAxis()),
            InputSource.ControllerButton => false,
            InputSource.ControllerAnalogButton => false,
            InputSource.ControllerAxis => false,
            _ => true,
        };
    }

    #endregion Queries

    #region Public

    public void EnableDebug()
    {
        _isDebugging = true;
    }

    public void DisableDebug()
    {
        _isDebugging = false;
    }
    
    #endregion Public

    #region Private

    private void SetKeyboard(IKeyboard? keyboard)
    {
        if (_keyboardState is not null && _keyboardState.Device == keyboard)
        {
            return;
        }

        _keyboardState?.Dispose();
        _keyboardState = null;

        if (keyboard is not null)
        {
            _keyboardState = new KeyboardState(keyboard);
        }
    }

    private void SetMouse(IMouse? mouse)
    {
        if (_mouseState is not null && _mouseState.Device == mouse)
        {
            return;
        }

        _mouseState?.Dispose();
        _mouseState = null;

        if (mouse is not null)
        {
            _mouseState = new MouseState(mouse);
        }
    }

    private void SetGamepad(IGamepad gamepad)
    {
        if (_activeGamepad == gamepad)
        {
            return;
        }
        
        if (_activeGamepad is not null)
        {
            _activeGamepad.ButtonDown -= OnButtonDown;
            _activeGamepad.ButtonUp -= OnButtonUp;
            _activeGamepad.ThumbstickMoved -= OnThumbstickMoved;
            _activeGamepad.TriggerMoved -= OnTriggerMoved;
        }

        _activeGamepad = gamepad;
        if (_activeGamepad is null)
        {
            return;
        }
        
        _activeGamepad.ButtonDown += OnButtonDown;
        _activeGamepad.ButtonUp += OnButtonUp;
        _activeGamepad.ThumbstickMoved += OnThumbstickMoved;
        _activeGamepad.TriggerMoved += OnTriggerMoved;
    }

    private void FindKeyboard(IKeyboard? preferredKeyboard)
    {
        if (preferredKeyboard is null && _inputContext.Keyboards.Count == 0)
        {
            _keyboardState = null;
        }

        SetKeyboard(preferredKeyboard ?? _inputContext.Keyboards[0]);
    }

    private void FindMouse(IMouse? preferredMouse)
    {
        if (preferredMouse is null && _inputContext.Mice.Count == 0)
        {
            _keyboardState = null;
        }

        SetMouse(preferredMouse ?? _inputContext.Mice[0]);
    }

    private void FindGamepad(IGamepad? preferredGamepad)
    {
        if (preferredGamepad is null && _inputContext.Gamepads.Count == 0)
        {
            _keyboardState = null;
        }

        SetGamepad(preferredGamepad ?? _inputContext.Gamepads[0]);
    }

    private void DebugPrint(string message)
    {
        if (_isDebugging)
        {
            Console.WriteLine($"[Input] {message}");
        }
    }

    private void DebugPrint(IInputDevice device, bool isConnected)
    {
        if (isConnected)
        {
            DebugPrint($"'{device.Name}' connected.");
        }
        else
        {
            DebugPrint($"'{device.Name}' disconnected.");
        }
    }

    #endregion Private

    #region Bindings

    private void OnConnectionChanged(IInputDevice device, bool isConnected)
    {
        switch (device)
        {
            case IKeyboard keyboard:
                DebugPrint(keyboard, isConnected);
                FindKeyboard(isConnected ? keyboard : null);
                return;
            case IMouse mouse:
                DebugPrint(mouse, isConnected);
                FindMouse(isConnected ? mouse : null);
                return;
            case IGamepad gamepad:
                DebugPrint(gamepad, isConnected);
                FindGamepad(isConnected ? gamepad : null);
                return;
        }
    }

    private void OnButtonDown(IGamepad gamepad, Button button)
    {
        DebugPrint($"Gamepad button `{button.Name}` pressed.");
    }

    private void OnButtonUp(IGamepad gamepad, Button button)
    {
        DebugPrint($"Gamepad button `{button.Name}` released.");
    }

    private void OnThumbstickMoved(IGamepad gamepad, Thumbstick thumbstick)
    {
        DebugPrint($"Gamepad thumbstick {thumbstick.Index} moved to `<{thumbstick.X} {thumbstick.Y}>`.");
    }

    private void OnTriggerMoved(IGamepad gamepad, Trigger trigger)
    {
        DebugPrint($"Gamepad thumbstick {trigger.Index} moved to `{trigger.Position}`.");
    }

    #endregion Bindings
}