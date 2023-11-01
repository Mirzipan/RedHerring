using RedHerring.Fingerprint.Events;
using RedHerring.Fingerprint.Layers;
using RedHerring.Fingerprint.Shortcuts;
using RedHerring.Fingerprint.States;
using Silk.NET.Input;
using Silk.NET.Input.Sdl;
using Silk.NET.Windowing;

namespace RedHerring.Fingerprint;

public partial class Input: IInput, IDisposable
{
    private IInputContext _inputContext;

    private KeyboardState? _keyboardState;
    private MouseState? _mouseState;
    private GamepadState? _gamepadState;
    
    private ActionsState _actionsState;

    private InputProcessor _processor;
    
    private bool _isDebugging;

    public bool IsDebugging => _isDebugging;

    public IKeyboardState? Keyboard => _keyboardState;
    public IMouseState? Mouse => _mouseState;
    public IGamepadState? Gamepad => _gamepadState;
    public IActionState Actions => _actionsState;

    public ShortcutBindings? Bindings { get; set; }
    public InputLayers Layers { get; }
    
    public event Action<KeyEvent>? KeyEvent;
    public event Action<MouseButtonEvent>? MouseButtonEvent;
    public event Action<MouseAxisEvent>? MouseAxisEvent;
    public event Action<GamepadButtonEvent>? GamepadButtonEvent;
    public event Action<GamepadAxisEvent>? GamepadAxisEvent;

    #region Lifecycle

    public Input(IView view)
    {
        SdlInput.RegisterPlatform();

        _inputContext = view.CreateInput();
        _inputContext.ConnectionChanged += OnConnectionChanged;

        Layers = new InputLayers();
        _actionsState = new ActionsState("actions");
        _processor = new InputProcessor(this, _actionsState);

        Bindings = new ShortcutBindings();
        
        InjectDebug();
        FindDevices();
    }

    public void Tick()
    {
        _processor.Tick();
        
        ResetStates();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        
        _inputContext.ConnectionChanged -= OnConnectionChanged;
        _inputContext.Dispose();
    }

    #endregion Lifecycle

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

    private void InjectDebug()
    {
        KeyboardState.DebugPrint = DebugPrint;
        MouseState.DebugPrint = DebugPrint;
        GamepadState.DebugPrint = DebugPrint;
    }

    private void FindDevices()
    {
        FindKeyboard(null);
        FindMouse(null);
        FindGamepad(null);
    }

    private void SetKeyboard(IKeyboard? keyboard)
    {
        if (_keyboardState?.Device == keyboard && keyboard is not null)
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
        if (_mouseState?.Device == mouse && mouse is not null)
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

    private void SetGamepad(IGamepad? gamepad)
    {
        if (_gamepadState?.Device == gamepad && gamepad is not null)
        {
            return;
        }

        _gamepadState?.Dispose();
        _gamepadState = null;

        if (gamepad is not null)
        {
            _gamepadState = new GamepadState(gamepad);
        }
    }

    private void FindKeyboard(IKeyboard? preferredKeyboard)
    {
        SetKeyboard(preferredKeyboard ?? _inputContext.Keyboards.FirstOrDefault());
    }

    private void FindMouse(IMouse? preferredMouse)
    {
        SetMouse(preferredMouse ?? _inputContext.Mice.FirstOrDefault());
    }

    private void FindGamepad(IGamepad? preferredGamepad)
    {
        SetGamepad(preferredGamepad ?? _inputContext.Gamepads.FirstOrDefault());
    }

    private void ResetStates()
    {
        _actionsState.Reset();
        
        _keyboardState?.Reset();
        _mouseState?.Reset();
        _gamepadState?.Reset();
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

    #endregion Bindings

    #region Logging

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

    #endregion Logging
}