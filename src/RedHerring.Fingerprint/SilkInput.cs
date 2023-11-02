using RedHerring.Alexandria.Extensions;
using RedHerring.Fingerprint.Events;
using RedHerring.Fingerprint.Layers;
using RedHerring.Fingerprint.Shortcuts;
using RedHerring.Fingerprint.States;
using Silk.NET.Input;
using Silk.NET.Input.Sdl;
using Silk.NET.Windowing;

namespace RedHerring.Fingerprint;

public partial class SilkInput: Input, IDisposable
{
    private IInputContext _inputContext;

    private KeyboardState? _keyboardState;
    private MouseState? _mouseState;
    private GamepadState? _gamepadState;
    
    private ActionsState _actionsState;

    private InputProcessor _processor;
    
    private bool _isDebugging;

    public bool IsDebugging => _isDebugging;

    public KeyboardState? Keyboard => _keyboardState;
    public MouseState? Mouse => _mouseState;
    public GamepadState? Gamepad => _gamepadState;
    public ActionsState Actions => _actionsState;

    public ShortcutBindings? Bindings { get; set; }
    public InputLayers Layers { get; }
    
    public event Action<KeyChanged>? KeyChange;
    public event Action<char>? KeyChar;
    public event Action<MouseButtonChanged>? MouseButtonChange;
    public event Action<MouseAxisMoved>? MouseAxisMove;
    public event Action<GamepadButtonChanged>? GamepadButtonChange;
    public event Action<GamepadAxisMoved>? GamepadAxisMove;

    #region Lifecycle

    public SilkInput(IView view)
    {
        SdlInput.RegisterPlatform();

        _inputContext = view.CreateInput();
        _inputContext.ConnectionChanged += OnConnectionChanged;

        Layers = new InputLayers();
        _actionsState = new ActionsState();
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

        if (_keyboardState is not null)
        {
            _keyboardState.KeyChange -= OnKeyChanged;
            _keyboardState.KeyChar -= OnKeyChar;
            _keyboardState.Dispose();
            _keyboardState = null;
        }

        if (keyboard is not null)
        {
            _keyboardState = new KeyboardState(keyboard);
            _keyboardState.KeyChange += OnKeyChanged;
            _keyboardState.KeyChar += OnKeyChar;
        }
    }

    private void SetMouse(IMouse? mouse)
    {
        if (_mouseState?.Mouse == mouse && mouse is not null)
        {
            return;
        }

        if (_mouseState is not null)
        {
            _mouseState.ButtonChange -= OnMouseButtonChanged;
            _mouseState.AxisMove -= OnMouseAxisMoved;
            _mouseState.Dispose();
            _mouseState = null;
        }

        if (mouse is not null)
        {
            _mouseState = new MouseState(mouse);
            _mouseState.ButtonChange += OnMouseButtonChanged;
            _mouseState.AxisMove += OnMouseAxisMoved;
        }
    }

    private void SetGamepad(IGamepad? gamepad)
    {
        if (_gamepadState?.Gamepad == gamepad && gamepad is not null)
        {
            return;
        }

        if (_gamepadState is not null)
        {
            _gamepadState.ButtonChange -= OnGamepadButtonChanged;
            _gamepadState.AxisMove -= OnGamepadAxisMoved;
            _gamepadState.Dispose();
            _gamepadState = null;
        }
        
        if (gamepad is not null)
        {
            _gamepadState = new GamepadState(gamepad);
            _gamepadState.ButtonChange += OnGamepadButtonChanged;
            _gamepadState.AxisMove += OnGamepadAxisMoved;
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

    private void OnKeyChanged(KeyChanged evt)
    {
        KeyChange.SafeInvoke(evt);
    }

    private void OnKeyChar(char @char)
    {
        KeyChar.SafeInvoke(@char);
    }

    private void OnMouseButtonChanged(MouseButtonChanged evt)
    {
        MouseButtonChange.SafeInvoke(evt with { Modifiers = _keyboardState?.Modifiers ?? Modifiers.None });
    }

    private void OnMouseAxisMoved(MouseAxisMoved evt)
    {
        MouseAxisMove.SafeInvoke(evt);
    }

    private void OnGamepadButtonChanged(GamepadButtonChanged evt)
    {
        GamepadButtonChange.SafeInvoke(evt with { Modifiers = _keyboardState?.Modifiers ?? Modifiers.None });
    }

    private void OnGamepadAxisMoved(GamepadAxisMoved evt)
    {
        GamepadAxisMove.SafeInvoke(evt);
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