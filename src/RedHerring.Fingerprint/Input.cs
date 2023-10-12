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
    private GamepadState? _gamepadState;
    
    private bool _isDebugging;

    public IKeyboardState? Keyboard => _keyboardState;
    public IMouseState? Mouse => _mouseState;
    public IGamepadState? Gamepad => _gamepadState;
    
    #region Lifecycle

    public Input(IView view)
    {
        SdlInput.RegisterPlatform();

        _inputContext = view.CreateInput();
        _inputContext.ConnectionChanged += OnConnectionChanged;

        InjectDebug();
        FindDevices();
    }

    public void Tick()
    {
        _keyboardState?.Update();
        _mouseState?.Update();
        _gamepadState?.Update();
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
    public void GetButtonsDown(IList<MouseButton> buttons) => _mouseState?.GetButtonsDown(buttons);
    public bool IsAnyMouseButtonDown() => _mouseState?.IsAnyButtonDown() ?? false;
    public bool IsMouseMoved(MouseAxis axis) => _mouseState?.IsMoved(axis) ?? false;
    public float GetAxis(MouseAxis axis) => _mouseState?.GetAxis(axis) ?? 0;

    public Vector2 MousePosition => _mouseState?.Position ?? Vector2.Zero;
    public Vector2 MouseDelta => _mouseState?.Delta ?? Vector2.Zero;
    public float MouseWheelDelta => _mouseState?.ScrollWheel.Y ?? 0;
    
    public bool IsButtonUp(GamepadButton button) => _gamepadState?.IsButtonUp(button) ?? true;
    public bool IsButtonPressed(GamepadButton button) => _gamepadState?.IsButtonPressed(button) ?? false;
    public bool IsButtonDown(GamepadButton button) => _gamepadState?.IsButtonDown(button) ?? false;
    public bool IsButtonReleased(GamepadButton button) => _gamepadState?.IsButtonReleased(button) ?? false;
    public bool IsAnyGamepadButtonDown() => _gamepadState?.IsAnyButtonDown() ?? false;
    public void GetButtonsDown(IList<GamepadButton> buttons) => _gamepadState?.GetButtonsDown(buttons);
    public float GetAxis(GamepadAxis axis) => _gamepadState?.GetAxis(axis) ?? 0;
    
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

    #endregion Bindings
}