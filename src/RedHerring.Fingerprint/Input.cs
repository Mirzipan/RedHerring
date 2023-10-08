using System.Numerics;
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

    public void Dispose()
    {
        _inputContext.ConnectionChanged -= OnConnectionChanged;
        _inputContext.Dispose();
    }

    #endregion Lifecycle

    #region Queries

    public bool IsKeyUp(Key key) => !_activeKeyboard?.IsKeyPressed(key) ?? true;
    public bool IsKeyDown(Key key) => _activeKeyboard?.IsKeyPressed(key) ?? false;

    public bool IsButtonUp(MouseButton button) => !_activeMouse?.IsButtonPressed(button) ?? true;
    public bool IsButtonDown(MouseButton button) => _activeMouse?.IsButtonPressed(button) ?? false;
    public Vector2 MousePosition => _activeMouse?.Position ?? Vector2.Zero;

    public bool IsButtonUp(ButtonName button) => !_activeGamepad?.Buttons.FirstOrDefault(e => e.Name == button).Pressed ?? true;
    public bool IsButtonDown(ButtonName button) => _activeGamepad?.Buttons.FirstOrDefault(e => e.Name == button).Pressed ?? false;

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
    }

    private void OnKeyUp(IKeyboard keyboard, Key key, int someValue)
    {
        DebugPrint($"Key `{key}` released (value = {someValue}).");
    }

    private void OnMouseMove(IMouse mouse, Vector2 position)
    {
        DebugPrint($"Mouse moved to `{position}`.");
    }

    private void OnMouseDown(IMouse mouse, MouseButton button)
    {
        DebugPrint($"Mouse button `{button}` pressed.");
    }

    private void OnMouseUp(IMouse mouse, MouseButton button)
    {
        DebugPrint($"Mouse button `{button}` pressed.");
    }

    private void OnMouseScroll(IMouse mouse, ScrollWheel scrollDelta)
    {
        DebugPrint($"Mouse scrolled by `{scrollDelta}`.");
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