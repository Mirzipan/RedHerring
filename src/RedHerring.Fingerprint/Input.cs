using System.Numerics;
using RedHerring.Extensions;
using Silk.NET.Input;
using Silk.NET.Input.Sdl;
using Silk.NET.Windowing;

namespace RedHerring.Fingerprint;

public class Input: IDisposable
{
    private IInputContext _inputContext;

    private IKeyboard? _activeKeyboard;
    private IMouse? _activeMouse;
    private IGamepad? _activeGamepad;

    private KeyboardState _lastKeyboardState;
    private KeyboardState _currentKeyboardState;

    private MouseState _lastMouseState;
    private MouseState _currentMouseState;

    private ControllerState _lastControllerState;
    private ControllerState _currentControllerState;
    
    private bool _isDebugging;
    
    #region Lifecycle

    public Input(IView view)
    {
        SdlInput.RegisterPlatform();

        _inputContext = view.CreateInput();
        _inputContext.ConnectionChanged += OnConnectionChanged;
        
        FindKeyboard(null);
        FindMouse(null);
        FindGamepad(null);
    }

    public void Tick()
    {
        _lastKeyboardState = _currentKeyboardState;
        _lastMouseState = _currentMouseState;
        _lastControllerState = _currentControllerState;
    }

    public void Dispose()
    {
        _inputContext.ConnectionChanged -= OnConnectionChanged;
        _inputContext.Dispose();
    }

    #endregion Lifecycle

    #region Queries

    public bool IsKeyUp(Key key) => _currentKeyboardState.IsKeyUp(key);
    
    public bool IsKeyPressed(Key key) => _lastKeyboardState.IsKeyUp(key) && _currentKeyboardState.IsKeyDown(key);

    public bool IsKeyDown(Key key) => _currentKeyboardState.IsKeyDown(key);

    public bool IsKeyReleased(Key key) => _lastKeyboardState.IsKeyDown(key) && _currentKeyboardState.IsKeyUp(key);

    public bool IsButtonUp(MouseButton button) => _currentMouseState.IsButtonUp(button);
    public bool IsButtonPressed(MouseButton button) => _lastMouseState.IsButtonUp(button) && _currentMouseState.IsButtonDown(button);
    public bool IsButtonDown(MouseButton button) => _currentMouseState.IsButtonDown(button);
    public bool IsButtonReleased(MouseButton button) => _lastMouseState.IsButtonDown(button) && _currentMouseState.IsButtonUp(button);

    public bool IsMouseMoved(MouseAxis axis)
    {
        return axis switch
        {
            MouseAxis.None => false,
            MouseAxis.Horizontal => !_currentMouseState.Position.X.Approximately(_lastMouseState.Position.X),
            MouseAxis.Vertical => !_currentMouseState.Position.Y.Approximately(_lastMouseState.Position.Y),
            MouseAxis.Wheel => _currentMouseState.ScrollWheel.Y != 0,
            MouseAxis.WheelUp => _currentMouseState.ScrollWheel.Y > float.Epsilon,
            MouseAxis.WheelDown => _currentMouseState.ScrollWheel.Y < -float.Epsilon,
            _ => false,
        };
    }
    
    public Vector2 MousePosition => _currentMouseState.Position;

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
        if (_activeKeyboard == keyboard)
        {
            return;
        }
        
        if (_activeKeyboard is not null)
        {
            _activeKeyboard.KeyDown -= OnKeyDown;
            _activeKeyboard.KeyUp -= OnKeyUp;
        }

        _activeKeyboard = keyboard;
        if (_activeKeyboard is null)
        {
            return;
        }
        
        _activeKeyboard.KeyDown += OnKeyDown;
        _activeKeyboard.KeyUp += OnKeyUp;
    }

    private void SetMouse(IMouse? mouse)
    {
        if (_activeMouse == mouse)
        {
            return;
        }
        
        if (_activeMouse is not null)
        {
            _activeMouse.MouseMove -= OnMouseMove;
            _activeMouse.MouseDown -= OnMouseDown;
            _activeMouse.MouseUp -= OnMouseUp;
            _activeMouse.Scroll -= OnMouseScroll;
        }

        _activeMouse = mouse;
        if (_activeMouse is null)
        {
            return;
        }
        
        _activeMouse.MouseMove += OnMouseMove;
        _activeMouse.MouseDown += OnMouseDown;
        _activeMouse.MouseUp += OnMouseUp;
        _activeMouse.Scroll += OnMouseScroll;
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
            _activeKeyboard = null;
        }

        SetKeyboard(preferredKeyboard ?? _inputContext.Keyboards[0]);
    }

    private void FindMouse(IMouse? preferredMouse)
    {
        if (preferredMouse is null && _inputContext.Mice.Count == 0)
        {
            _activeKeyboard = null;
        }

        SetMouse(preferredMouse ?? _inputContext.Mice[0]);
    }

    private void FindGamepad(IGamepad? preferredGamepad)
    {
        if (preferredGamepad is null && _inputContext.Gamepads.Count == 0)
        {
            _activeKeyboard = null;
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

    private void OnKeyDown(IKeyboard keyboard, Key key, int someValue)
    {
        DebugPrint($"Key `{key}` pressed (value = {someValue}).");
        _currentKeyboardState.SetKey(key, true);
    }

    private void OnKeyUp(IKeyboard keyboard, Key key, int someValue)
    {
        DebugPrint($"Key `{key}` released (value = {someValue}).");
        _currentKeyboardState.SetKey(key, false);
    }

    private void OnMouseMove(IMouse mouse, Vector2 position)
    {
        DebugPrint($"Mouse moved to `{position}`.");
        _currentMouseState.Position = position;
    }

    private void OnMouseDown(IMouse mouse, MouseButton button)
    {
        DebugPrint($"Mouse button `{button}` pressed.");
        _currentMouseState.SetButton(button, true);
    }

    private void OnMouseUp(IMouse mouse, MouseButton button)
    {
        DebugPrint($"Mouse button `{button}` pressed.");
        _currentMouseState.SetButton(button, false);
    }

    private void OnMouseScroll(IMouse mouse, ScrollWheel scrollDelta)
    {
        DebugPrint($"Mouse scrolled by `{scrollDelta}`.");
        _currentMouseState.ScrollWheel = new Vector2(scrollDelta.X, scrollDelta.Y);
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