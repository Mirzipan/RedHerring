using System.Collections;
using Silk.NET.Input;
using SilkKey = Silk.NET.Input.Key;

namespace RedHerring.Fingerprint.States;

internal class KeyboardState : IInputSource, IKeyboardState, IDisposable
{
    private const int BufferLength = 512;
    private const int BitsPerByte = 8;

    internal static Action<string>? DebugPrint;
    private static int[] _tmpBuffer = new int[BufferLength / sizeof(int) / BitsPerByte];

    private readonly BitArray _down = new(BufferLength);
    private readonly BitArray _pressed = new(BufferLength);
    private readonly BitArray _released = new(BufferLength);
    
    private IKeyboard _keyboard;

    public string Name => _keyboard.Name;
    public int Priority { get; set; }
    IInputDevice IInputSource.Device => _keyboard;
    public IKeyboard Device => _keyboard;

    #region Lifecycle

    public KeyboardState(IKeyboard keyboard)
    {
        _keyboard = keyboard;
        
        _keyboard.KeyDown += OnKeyDown;
        _keyboard.KeyUp += OnKeyUp;
    }

    public void Reset()
    {
        _pressed.SetAll(false);
        _released.SetAll(false);
    }

    public void Dispose()
    {
        _keyboard.KeyDown -= OnKeyDown;
        _keyboard.KeyUp -= OnKeyUp;

        _keyboard = null!;
    }

    #endregion Lifecycle

    #region Queries

    public bool IsKeyUp(Key key) => !_down.Get((int)key);
    public bool IsKeyPressed(Key key) => _pressed.Get((int)key);
    public bool IsKeyDown(Key key) => _down.Get((int)key);
    public bool IsKeyReleased(Key key) => _released.Get((int)key);
    public bool IsAnyKeyDown() => IsBufferEmpty();
    
    public void GetKeysDown(IList<Key> keys)
    {
        for (int i = 0; i < BufferLength; i++)
        {
            if (_down[i])
            {
                keys.Add((Key)i);
            }
        }
    }

    #endregion Queries

    #region Private

    private static Key ConvertKey(SilkKey key)
    {
        return key switch
        {
            SilkKey.Unknown => Key.Unknown,
            SilkKey.Space => Key.Space,
            SilkKey.Apostrophe => Key.Apostrophe,
            SilkKey.Comma => Key.Comma,
            SilkKey.Minus => Key.Minus,
            SilkKey.Period => Key.Period,
            SilkKey.Slash => Key.Slash,
            SilkKey.Number0 => Key.Number0,
            SilkKey.Number1 => Key.Number1,
            SilkKey.Number2 => Key.Number2,
            SilkKey.Number3 => Key.Number3,
            SilkKey.Number4 => Key.Number4,
            SilkKey.Number5 => Key.Number5,
            SilkKey.Number6 => Key.Number6,
            SilkKey.Number7 => Key.Number7,
            SilkKey.Number8 => Key.Number8,
            SilkKey.Number9 => Key.Number9,
            SilkKey.Semicolon => Key.Semicolon,
            SilkKey.Equal => Key.Equal,
            SilkKey.A => Key.A,
            SilkKey.B => Key.B,
            SilkKey.C => Key.C,
            SilkKey.D => Key.D,
            SilkKey.E => Key.E,
            SilkKey.F => Key.F,
            SilkKey.G => Key.G,
            SilkKey.H => Key.H,
            SilkKey.I => Key.I,
            SilkKey.J => Key.J,
            SilkKey.K => Key.K,
            SilkKey.L => Key.L,
            SilkKey.M => Key.M,
            SilkKey.N => Key.N,
            SilkKey.O => Key.O,
            SilkKey.P => Key.P,
            SilkKey.Q => Key.Q,
            SilkKey.R => Key.R,
            SilkKey.S => Key.S,
            SilkKey.T => Key.T,
            SilkKey.U => Key.U,
            SilkKey.V => Key.V,
            SilkKey.W => Key.W,
            SilkKey.X => Key.X,
            SilkKey.Y => Key.Y,
            SilkKey.Z => Key.Z,
            SilkKey.LeftBracket => Key.LeftBracket,
            SilkKey.BackSlash => Key.BackSlash,
            SilkKey.RightBracket => Key.RightBracket,
            SilkKey.GraveAccent => Key.GraveAccent,
            SilkKey.World1 => Key.World1,
            SilkKey.World2 => Key.World2,
            SilkKey.Escape => Key.Escape,
            SilkKey.Enter => Key.Enter,
            SilkKey.Tab => Key.Tab,
            SilkKey.Backspace => Key.Backspace,
            SilkKey.Insert => Key.Insert,
            SilkKey.Delete => Key.Delete,
            SilkKey.Right => Key.Right,
            SilkKey.Left => Key.Left,
            SilkKey.Down => Key.Down,
            SilkKey.Up => Key.Up,
            SilkKey.PageUp => Key.PageUp,
            SilkKey.PageDown => Key.PageDown,
            SilkKey.Home => Key.Home,
            SilkKey.End => Key.End,
            SilkKey.CapsLock => Key.CapsLock,
            SilkKey.ScrollLock => Key.ScrollLock,
            SilkKey.NumLock => Key.NumLock,
            SilkKey.PrintScreen => Key.PrintScreen,
            SilkKey.Pause => Key.Pause,
            SilkKey.F1 => Key.F1,
            SilkKey.F2 => Key.F2,
            SilkKey.F3 => Key.F3,
            SilkKey.F4 => Key.F4,
            SilkKey.F5 => Key.F5,
            SilkKey.F6 => Key.F6,
            SilkKey.F7 => Key.F7,
            SilkKey.F8 => Key.F8,
            SilkKey.F9 => Key.F9,
            SilkKey.F10 => Key.F10,
            SilkKey.F11 => Key.F11,
            SilkKey.F12 => Key.F12,
            SilkKey.F13 => Key.F13,
            SilkKey.F14 => Key.F14,
            SilkKey.F15 => Key.F15,
            SilkKey.F16 => Key.F16,
            SilkKey.F17 => Key.F17,
            SilkKey.F18 => Key.F18,
            SilkKey.F19 => Key.F19,
            SilkKey.F20 => Key.F20,
            SilkKey.F21 => Key.F21,
            SilkKey.F22 => Key.F22,
            SilkKey.F23 => Key.F23,
            SilkKey.F24 => Key.F24,
            SilkKey.F25 => Key.F25,
            SilkKey.Keypad0 => Key.Keypad0,
            SilkKey.Keypad1 => Key.Keypad1,
            SilkKey.Keypad2 => Key.Keypad2,
            SilkKey.Keypad3 => Key.Keypad3,
            SilkKey.Keypad4 => Key.Keypad4,
            SilkKey.Keypad5 => Key.Keypad5,
            SilkKey.Keypad6 => Key.Keypad6,
            SilkKey.Keypad7 => Key.Keypad7,
            SilkKey.Keypad8 => Key.Keypad8,
            SilkKey.Keypad9 => Key.Keypad9,
            SilkKey.KeypadDecimal => Key.KeypadDecimal,
            SilkKey.KeypadDivide => Key.KeypadDivide,
            SilkKey.KeypadMultiply => Key.KeypadMultiply,
            SilkKey.KeypadSubtract => Key.KeypadSubtract,
            SilkKey.KeypadAdd => Key.KeypadAdd,
            SilkKey.KeypadEnter => Key.KeypadEnter,
            SilkKey.KeypadEqual => Key.KeypadEqual,
            SilkKey.ShiftLeft => Key.ShiftLeft,
            SilkKey.ControlLeft => Key.ControlLeft,
            SilkKey.AltLeft => Key.AltLeft,
            SilkKey.SuperLeft => Key.SuperLeft,
            SilkKey.ShiftRight => Key.ShiftRight,
            SilkKey.ControlRight => Key.ControlRight,
            SilkKey.AltRight => Key.AltRight,
            SilkKey.SuperRight => Key.SuperRight,
            SilkKey.Menu => Key.Menu,
            _ => Key.Unknown,
        };
    }
    
    private bool IsBufferEmpty()
    {
        _down.CopyTo(_tmpBuffer, 0);
        return _tmpBuffer.All(value => value == 0);
    }

    #endregion Private

    #region Bindings

    private void OnKeyDown(IKeyboard keyboard, SilkKey silkKey, int keyCode)
    {
        var key = ConvertKey(silkKey);
        _pressed[(int)key] = true;
        _down[(int)key] = true;
        
        DebugPrint?.Invoke($"`{keyboard.Name}` key `{key}` pressed (keyCode = {keyCode}).");
    }

    private void OnKeyUp(IKeyboard keyboard, SilkKey silkKey, int keyCode)
    {
        var key = ConvertKey(silkKey);
        _down[(int)key] = false;
        _released[(int)key] = true;
        
        DebugPrint?.Invoke($"`{keyboard.Name}` key `{key}` released (keyCode = {keyCode}).");
    }

    #endregion Bindings
}