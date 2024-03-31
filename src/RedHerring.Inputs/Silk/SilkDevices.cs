using System.Numerics;
using Silk.NET.Input;
using Silk.NET.Input.Sdl;
using Silk.NET.Windowing;

namespace RedHerring.Inputs;

public sealed class SilkDevices : InputDevices, IDisposable
{
    private readonly IInputContext _silkContext;

    private IKeyboard? _keyboard;
    private IMouse? _mouse;
    private IGamepad? _gamepad;

    public event Action<InputChanged>? InputChanged;
    public event Action<int, char>? CharacterTyped;

    #region Lifecycle

    public SilkDevices(IView view) 
    {
        SdlInput.RegisterPlatform();
        
        _silkContext = view.CreateInput();
        _silkContext.ConnectionChanged += OnConnectionChanged;
        
        PopulateDefaults();
    }

    ~SilkDevices()
    {
        
    }

    public void NextFrame()
    {
    }
    
    public void Cursor(CursorKind cursor)
    {
        if (_mouse is null)
        {
            return;
        }
        
        _mouse.Cursor.StandardCursor = SilkConvert.ToStandardCursor(cursor);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        
        _silkContext.Dispose();
    }

    #endregion Lifecycle
    
    #region Devices

    private void OnConnectionChanged(IInputDevice device, bool isConnected)
    {
        switch (device)
        {
            case IKeyboard keyboard:
                if (!isConnected)
                {
                    SetKeyboard(_silkContext.Keyboards.Count > 0 ? _silkContext.Keyboards[0] : null);
                }
                else
                {
                    SetKeyboard(keyboard);
                }
                return;
            case IMouse mouse:
                if (!isConnected)
                {
                    SetMouse(_silkContext.Mice.Count > 0 ? _silkContext.Mice[0] : null);
                }
                else
                {
                    SetMouse(mouse);
                }
                return;
            case IGamepad gamepad:
                if (!isConnected)
                {
                    SetGamepad(_silkContext.Gamepads.Count > 0 ? _silkContext.Gamepads[0] : null);
                }
                else
                {
                    SetGamepad(gamepad);
                }
                return;
        }
    }

    private void PopulateDefaults()
    {
        SetKeyboard(_silkContext.Keyboards.Count > 0 ? _silkContext.Keyboards[0] : null);
        SetMouse(_silkContext.Mice.Count > 0 ? _silkContext.Mice[0] : null);
        SetGamepad(_silkContext.Gamepads.Count > 0 ? _silkContext.Gamepads[0] : null);
    }

    #endregion Devices

    #region Keyboard
    
    private void SetKeyboard(IKeyboard? keyboard)
    {
        if (_keyboard is not null)
        {
            UnbindKeyboard(_keyboard);
        }

        _keyboard = keyboard;
        if (keyboard is not null)
        {
            BindKeyboard(keyboard);
        }
    }

    private void BindKeyboard(IKeyboard keyboard)
    {
        keyboard.KeyDown += OnKeyDown;
        keyboard.KeyUp += OnKeyUp;
        keyboard.KeyChar += OnKeyChar;
    }

    private void UnbindKeyboard(IKeyboard keyboard)
    {
        keyboard.KeyDown -= OnKeyDown;
        keyboard.KeyUp -= OnKeyUp;
        keyboard.KeyChar -= OnKeyChar;
    }

    private void OnKeyDown(IKeyboard keyboard, Key key, int scanCode)
    {
        var evt = new InputChanged(keyboard.Index, SilkConvert.FromKey(key), true, 1.00f);
        InputChanged?.Invoke(evt);
    }

    private void OnKeyUp(IKeyboard keyboard, Key key, int scanCode)
    {
        var evt = new InputChanged(keyboard.Index, SilkConvert.FromKey(key), false, 0.00f);
        InputChanged?.Invoke(evt);
    }

    private void OnKeyChar(IKeyboard keyboard, char character)
    {
        CharacterTyped?.Invoke(keyboard.Index, character);
    }

    #endregion Keyboard

    #region Mouse
    
    private void SetMouse(IMouse? mouse)
    {
        if (_mouse is not null)
        {
            UnbindMouse(_mouse);
        }

        _mouse = mouse;
        if (mouse is not null)
        {
            BindMouse(mouse);
        }
    }

    private void BindMouse(IMouse mouse)
    {
        mouse.MouseMove += OnMouseMove;
        mouse.MouseDown += OnMouseDown;
        mouse.MouseUp += OnMouseUp;
        mouse.Scroll += OnMouseScroll;
    }

    private void UnbindMouse(IMouse mouse)
    {
        mouse.MouseMove -= OnMouseMove;
        mouse.MouseDown -= OnMouseDown;
        mouse.MouseUp -= OnMouseUp;
        mouse.Scroll -= OnMouseScroll;
    }

    private void OnMouseMove(IMouse mouse, Vector2 position)
    {
        InputChanged?.Invoke(new InputChanged(mouse.Index, Input.MouseX, true, position.X));
        InputChanged?.Invoke(new InputChanged(mouse.Index, Input.MouseY, true, position.Y));
    }

    private void OnMouseDown(IMouse mouse, MouseButton button)
    {
        var evt = new InputChanged(mouse.Index, SilkConvert.FromMouseButton(button), true, 1.00f);
        InputChanged?.Invoke(evt);
    }

    private void OnMouseUp(IMouse mouse, MouseButton button)
    {
        var evt = new InputChanged(mouse.Index, SilkConvert.FromMouseButton(button), false, 0.00f);
        InputChanged?.Invoke(evt);
    }

    private void OnMouseScroll(IMouse mouse, ScrollWheel scrollDelta)
    {
        InputChanged?.Invoke(new InputChanged(mouse.Index, Input.MouseWheelX, true, scrollDelta.X));
        InputChanged?.Invoke(new InputChanged(mouse.Index, Input.MouseWheelY, true, scrollDelta.Y));
    }

    #endregion Mouse

    #region Gamepad

    private void SetGamepad(IGamepad? gamepad)
    {
        if (_gamepad is not null)
        {
            UnbindGamepad(_gamepad);
        }

        _gamepad = gamepad;
        if (gamepad is not null)
        {
            BindGamepad(gamepad);
        }
    }

    private void BindGamepad(IGamepad gamepad)
    {
        gamepad.ButtonDown += OnButtonDown;
        gamepad.ButtonUp += OnButtonUp;
        gamepad.ThumbstickMoved += OnThumbstickMoved;
        gamepad.TriggerMoved += OnTriggerMoved;
    }

    private void UnbindGamepad(IGamepad gamepad)
    {
        gamepad.ButtonDown -= OnButtonDown;
        gamepad.ButtonUp -= OnButtonUp;
        gamepad.ThumbstickMoved -= OnThumbstickMoved;
        gamepad.TriggerMoved -= OnTriggerMoved;
    }

    private void OnButtonDown(IGamepad gamepad, Button button)
    {
        var evt = new InputChanged(gamepad.Index, SilkConvert.FromGamepadButton(button), true, 1.00f);
        InputChanged?.Invoke(evt);
    }

    private void OnButtonUp(IGamepad gamepad, Button button)
    {
        var evt = new InputChanged(gamepad.Index, SilkConvert.FromGamepadButton(button), false, 0.00f);
        InputChanged?.Invoke(evt);
    }

    private void OnThumbstickMoved(IGamepad gamepad, Thumbstick thumbstick)
    {
        (Input stickX, Input stickY) = thumbstick.Index switch
        {
            0 => (Input.GamepadStickLeftX, Input.GamepadStickLeftY),
            1 => (Input.GamepadStickRightX, Input.GamepadStickRightY),
            _ => (Input.Unknown, Input.Unknown),
        };
        
        InputChanged?.Invoke(new InputChanged(gamepad.Index, stickX, true, thumbstick.X));
        InputChanged?.Invoke(new InputChanged(gamepad.Index, stickY, true, thumbstick.Y));
    }

    private void OnTriggerMoved(IGamepad gamepad, Trigger trigger)
    {
        Input triggerInput = trigger.Index switch
        {
            0 => Input.GamepadTriggerLeft,
            1 => Input.GamepadTriggerRight,
            _ => Input.Unknown,
        };
        
        InputChanged?.Invoke(new InputChanged(gamepad.Index, triggerInput, true, trigger.Position));
    }

    #endregion Gamepad
}