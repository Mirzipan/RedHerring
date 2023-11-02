using System.Numerics;
using RedHerring.Alexandria.Masks;
using RedHerring.Fingerprint;
using RedHerring.Fingerprint.States;
using Veldrid;
using Key = RedHerring.Fingerprint.Key;
using MouseButton = RedHerring.Fingerprint.MouseButton;
using VKey = Veldrid.Key;
using VMouseButton = Veldrid.MouseButton;

namespace RedHerring.ImGui;

internal class ImInputSnapshot : InputSnapshot
{
    private static readonly List<MouseButton> TmpButtons = new();
    private static readonly List<Key> TmpKeys = new();
    
    private readonly List<KeyEvent> _keyEvents = new();
    private readonly List<MouseEvent> _mouseEvents = new();
    private readonly List<char> _keyCharPresses = new();

    private BitMask8 _mouseButtons = new BitMask8(false);

    private Vector2 _mousePosition;
    private float _wheelDelta;

    public IReadOnlyList<KeyEvent> KeyEvents => _keyEvents;
    public IReadOnlyList<MouseEvent> MouseEvents => _mouseEvents;
    public IReadOnlyList<char> KeyCharPresses => _keyCharPresses;
    public Vector2 MousePosition => _mousePosition;
    public float WheelDelta => _wheelDelta;

    public void Update(IInput input)
    {
        UpdateMouse(input);
        UpdateKeyboard(input);
    }

    public bool IsMouseDown(VMouseButton button) => _mouseButtons[(int)button];

    #region Keyboard

    private void UpdateKeyboard(IInput input)
    {
        _keyEvents.Clear();
        _keyCharPresses.Clear();

        var keyboard = input.Keyboard;
        if (keyboard is not null)
        {
            CreatePressedKeyEvents(keyboard);
            CreateReleasedKeyEvents(keyboard);
            UpdateChars(keyboard);
        }
    }

    private void CreatePressedKeyEvents(KeyboardState keyboard)
    {
        TmpKeys.Clear();
        var modifiers = Modifiers(keyboard);
        
        keyboard.KeysPressed(TmpKeys);
        foreach (var key in TmpKeys)
        {
            _keyEvents.Add(new KeyEvent(Convert(key), true, modifiers));
        }
    }

    private void CreateReleasedKeyEvents(KeyboardState keyboard)
    {
        TmpKeys.Clear();
        var modifiers = Modifiers(keyboard);
        
        keyboard.KeysReleased(TmpKeys);
        foreach (var key in TmpKeys)
        {
            _keyEvents.Add(new KeyEvent(Convert(key), true, modifiers));
        }
    }

    private void UpdateChars(KeyboardState keyboard)
    {
        keyboard.Chars(_keyCharPresses);
    }

    private ModifierKeys Modifiers(KeyboardState keyboard)
    {
        var result = ModifierKeys.None;

        if (keyboard.IsKeyDown(Key.ShiftLeft) || keyboard.IsKeyDown(Key.ShiftRight))
        {
            result |= ModifierKeys.Shift;
        }

        if (keyboard.IsKeyDown(Key.ControlLeft) || keyboard.IsKeyDown(Key.ControlRight))
        {
            result |= ModifierKeys.Control;
        }

        if (keyboard.IsKeyDown(Key.AltLeft) || keyboard.IsKeyDown(Key.AltRight))
        {
            result |= ModifierKeys.Alt;
        }

        if (keyboard.IsKeyDown(Key.SuperLeft) || keyboard.IsKeyDown(Key.SuperRight))
        {
            result |= ModifierKeys.Gui;
        }

        return result;
    }

    #endregion Keyboard

    #region Mouse

    private void UpdateMouse(IInput input)
    {
        _mouseEvents.Clear();
        _mouseButtons = BitMask8.Empty;

        var mouse = input.Mouse;
        if (mouse is not null)
        {
            CreateMouseEvents(mouse);
            UpdateMouseButtons(mouse);
            UpdateMouseDeltas(input);
        }
    }

    private void CreateMouseEvents(MouseState mouse)
    {
        const int maxButton = (int)MouseButton.Button5;
        for (int i = 0; i <= maxButton; i++)
        {
            if (mouse.IsButtonPressed((MouseButton)i))
            {
                _mouseEvents.Add(new MouseEvent((VMouseButton)i, true));
            }

            if (mouse.IsButtonReleased((MouseButton)i))
            {
                _mouseEvents.Add(new MouseEvent((VMouseButton)i, false));
            }
        }
    }

    private void UpdateMouseButtons(MouseState mouse)
    {
        TmpButtons.Clear();

        mouse.ButtonsDown(TmpButtons);
        foreach (var button in TmpButtons)
        {
            _mouseButtons[(int)button] = true;
        }
    }

    private void UpdateMouseDeltas(IInput input)
    {
        _mousePosition = input.MousePosition;
        _wheelDelta = input.MouseWheelDelta;
    }

    #endregion Mouse

    private static VKey Convert(Key key)
    {
        return key switch
        {
            Key.Unknown => VKey.Unknown,
            Key.ShiftLeft => VKey.ShiftLeft,
            Key.ShiftRight => VKey.ShiftRight,
            Key.ControlLeft => VKey.ControlLeft,
            Key.ControlRight => VKey.ControlRight,
            Key.AltLeft => VKey.AltLeft,
            Key.AltRight => VKey.AltRight,
            Key.SuperLeft => VKey.WinLeft,
            Key.SuperRight => VKey.WinRight,
            Key.Menu => VKey.Menu,
            Key.F1 => VKey.F1,
            Key.F2 => VKey.F2,
            Key.F3 => VKey.F3,
            Key.F4 => VKey.F4,
            Key.F5 => VKey.F5,
            Key.F6 => VKey.F6,
            Key.F7 => VKey.F7,
            Key.F8 => VKey.F8,
            Key.F9 => VKey.F9,
            Key.F10 => VKey.F10,
            Key.F11 => VKey.F11,
            Key.F12 => VKey.F12,
            Key.F13 => VKey.F13,
            Key.F14 => VKey.F14,
            Key.F15 => VKey.F15,
            Key.F16 => VKey.F16,
            Key.F17 => VKey.F17,
            Key.F18 => VKey.F18,
            Key.F19 => VKey.F19,
            Key.F20 => VKey.F20,
            Key.F21 => VKey.F21,
            Key.F22 => VKey.F22,
            Key.F23 => VKey.F23,
            Key.F24 => VKey.F24,
            Key.F25 => VKey.F25,
            // Key.F26 => VKey.F26,
            // Key.F27 => VKey.F27,
            // Key.F28 => VKey.F28,
            // Key.F29 => VKey.F29,
            // Key.F30 => VKey.F30,
            // Key.F31 => VKey.F31,
            // Key.F32 => VKey.F32,
            // Key.F33 => VKey.F33,
            // Key.F34 => VKey.F34,
            // Key.F35 => VKey.F35,
            Key.Up => VKey.Up,
            Key.Down => VKey.Down,
            Key.Left => VKey.Left,
            Key.Right => VKey.Right,
            Key.Enter => VKey.Enter,
            Key.Escape => VKey.Escape,
            Key.Space => VKey.Space,
            Key.Tab => VKey.Tab,
            Key.Backspace => VKey.BackSpace,
            Key.Insert => VKey.Insert,
            Key.Delete => VKey.Delete,
            Key.PageUp => VKey.PageUp,
            Key.PageDown => VKey.PageDown,
            Key.Home => VKey.Home,
            Key.End => VKey.End,
            Key.CapsLock => VKey.CapsLock,
            Key.ScrollLock => VKey.ScrollLock,
            Key.PrintScreen => VKey.PrintScreen,
            Key.Pause => VKey.Pause,
            Key.NumLock => VKey.NumLock,
            // Key.Clear => VKey.Clear,
            // Key.Sleep => VKey.Sleep,
            Key.Keypad0 => VKey.Keypad0,
            Key.Keypad1 => VKey.Keypad1,
            Key.Keypad2 => VKey.Keypad2,
            Key.Keypad3 => VKey.Keypad3,
            Key.Keypad4 => VKey.Keypad4,
            Key.Keypad5 => VKey.Keypad5,
            Key.Keypad6 => VKey.Keypad6,
            Key.Keypad7 => VKey.Keypad7,
            Key.Keypad8 => VKey.Keypad8,
            Key.Keypad9 => VKey.Keypad9,
            Key.KeypadDivide => VKey.KeypadDivide,
            Key.KeypadMultiply => VKey.KeypadMultiply,
            Key.KeypadSubtract => VKey.KeypadSubtract,
            Key.KeypadAdd => VKey.KeypadAdd,
            Key.KeypadDecimal => VKey.KeypadDecimal,
            Key.KeypadEnter => VKey.KeypadEnter,
            Key.A => VKey.A,
            Key.B => VKey.B,
            Key.C => VKey.C,
            Key.D => VKey.D,
            Key.E => VKey.E,
            Key.F => VKey.F,
            Key.G => VKey.G,
            Key.H => VKey.H,
            Key.I => VKey.I,
            Key.J => VKey.J,
            Key.K => VKey.K,
            Key.L => VKey.L,
            Key.M => VKey.M,
            Key.N => VKey.N,
            Key.O => VKey.O,
            Key.P => VKey.P,
            Key.Q => VKey.Q,
            Key.R => VKey.R,
            Key.S => VKey.S,
            Key.T => VKey.T,
            Key.U => VKey.U,
            Key.V => VKey.V,
            Key.W => VKey.W,
            Key.X => VKey.X,
            Key.Y => VKey.Y,
            Key.Z => VKey.Z,
            Key.Number0 => VKey.Number0,
            Key.Number1 => VKey.Number1,
            Key.Number2 => VKey.Number2,
            Key.Number3 => VKey.Number3,
            Key.Number4 => VKey.Number4,
            Key.Number5 => VKey.Number5,
            Key.Number6 => VKey.Number6,
            Key.Number7 => VKey.Number7,
            Key.Number8 => VKey.Number8,
            Key.Number9 => VKey.Number9,
            Key.GraveAccent => VKey.Tilde,
            Key.Minus => VKey.Minus,
            Key.Equal => VKey.Plus,
            Key.LeftBracket => VKey.BracketLeft,
            Key.RightBracket => VKey.BracketRight,
            Key.Semicolon => VKey.Semicolon,
            Key.Apostrophe => VKey.Quote,
            Key.Comma => VKey.Comma,
            Key.Period => VKey.Period,
            Key.Slash => VKey.Slash,
            Key.BackSlash => VKey.BackSlash,
            //Key.NonUSBackSlash => VKey.NonUSBackSlash,
            _ => VKey.Unknown,
        };
    }

    private static MouseButton Convert(VMouseButton button)
    {
        return button switch
        {
            VMouseButton.Left => MouseButton.Left,
            VMouseButton.Middle => MouseButton.Middle,
            VMouseButton.Right => MouseButton.Right,
            VMouseButton.Button1 => MouseButton.Button4,
            VMouseButton.Button2 => MouseButton.Button5,
            VMouseButton.Button3 => MouseButton.Button6,
            VMouseButton.Button4 => MouseButton.Button7,
            VMouseButton.Button5 => MouseButton.Button8,
            VMouseButton.Button6 => MouseButton.Button9,
            VMouseButton.Button7 => MouseButton.Button10,
            VMouseButton.Button8 => MouseButton.Button11,
            VMouseButton.Button9 => MouseButton.Button12,
            VMouseButton.LastButton => MouseButton.Button12,
            _ => MouseButton.Unknown,
        };
    }

}