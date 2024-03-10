﻿using Silk.NET.Input;

namespace RedHerring.Fingerprint;

internal static class Convert
{
    public static Input FromKey(Key key)
    {
        return key switch
        {
            Key.Unknown => Input.Unknown,
            Key.Space => Input.Space,
            Key.Apostrophe => Input.Apostrophe,
            Key.Comma => Input.Comma,
            Key.Minus => Input.Minus,
            Key.Period => Input.Period,
            Key.Slash => Input.Slash,
            Key.Number0 => Input.Number0,
            Key.Number1 => Input.Number1,
            Key.Number2 => Input.Number2,
            Key.Number3 => Input.Number3,
            Key.Number4 => Input.Number4,
            Key.Number5 => Input.Number5,
            Key.Number6 => Input.Number6,
            Key.Number7 => Input.Number7,
            Key.Number8 => Input.Number8,
            Key.Number9 => Input.Number9,
            Key.Semicolon => Input.Semicolon,
            Key.Equal => Input.Equal,
            Key.A => Input.A,
            Key.B => Input.B,
            Key.C => Input.C,
            Key.D => Input.D,
            Key.E => Input.E,
            Key.F => Input.F,
            Key.G => Input.G,
            Key.H => Input.H,
            Key.I => Input.I,
            Key.J => Input.J,
            Key.K => Input.K,
            Key.L => Input.L,
            Key.M => Input.M,
            Key.N => Input.N,
            Key.O => Input.O,
            Key.P => Input.P,
            Key.Q => Input.Q,
            Key.R => Input.R,
            Key.S => Input.S,
            Key.T => Input.T,
            Key.U => Input.U,
            Key.V => Input.V,
            Key.W => Input.W,
            Key.X => Input.X,
            Key.Y => Input.Y,
            Key.Z => Input.Z,
            Key.LeftBracket => Input.BracketLeft,
            Key.BackSlash => Input.BackSlash,
            Key.RightBracket => Input.BracketRight,
            Key.GraveAccent => Input.Backquote,
            Key.World1 => Input.Oper,
            Key.World2 => Input.ClearAgain,
            Key.Escape => Input.Escape,
            Key.Enter => Input.Return,
            Key.Tab => Input.Tab,
            Key.Backspace => Input.Backspace,
            Key.Insert => Input.Insert,
            Key.Delete => Input.Delete,
            Key.Right => Input.Right,
            Key.Left => Input.Left,
            Key.Down => Input.Down,
            Key.Up => Input.Up,
            Key.PageUp => Input.PageUp,
            Key.PageDown => Input.PageDown,
            Key.Home => Input.Home,
            Key.End => Input.End,
            Key.CapsLock => Input.CapsLock,
            Key.ScrollLock => Input.ScrollLock,
            Key.NumLock => Input.NumLock,
            Key.PrintScreen => Input.PrintScreen,
            Key.Pause => Input.Pause,
            Key.F1 => Input.F1,
            Key.F2 => Input.F2,
            Key.F3 => Input.F3,
            Key.F4 => Input.F4,
            Key.F5 => Input.F5,
            Key.F6 => Input.F6,
            Key.F7 => Input.F7,
            Key.F8 => Input.F8,
            Key.F9 => Input.F9,
            Key.F10 => Input.F10,
            Key.F11 => Input.F11,
            Key.F12 => Input.F12,
            Key.F13 => Input.F13,
            Key.F14 => Input.F14,
            Key.F15 => Input.F15,
            Key.F16 => Input.F16,
            Key.F17 => Input.F17,
            Key.F18 => Input.F18,
            Key.F19 => Input.F19,
            Key.F20 => Input.F20,
            Key.F21 => Input.F21,
            Key.F22 => Input.F22,
            Key.F23 => Input.F23,
            Key.F24 => Input.F24,
            Key.F25 => Input.Execute,
            Key.Keypad0 => Input.Keypad0,
            Key.Keypad1 => Input.Keypad1,
            Key.Keypad2 => Input.Keypad2,
            Key.Keypad3 => Input.Keypad3,
            Key.Keypad4 => Input.Keypad4,
            Key.Keypad5 => Input.Keypad5,
            Key.Keypad6 => Input.Keypad6,
            Key.Keypad7 => Input.Keypad7,
            Key.Keypad8 => Input.Keypad8,
            Key.Keypad9 => Input.Keypad9,
            Key.KeypadDecimal => Input.KeypadDecimal,
            Key.KeypadDivide => Input.KeypadDivide,
            Key.KeypadMultiply => Input.KeypadMultiply,
            Key.KeypadSubtract => Input.KeypadMinus,
            Key.KeypadAdd => Input.KeypadPlus,
            Key.KeypadEnter => Input.KeypadEnter,
            Key.KeypadEqual => Input.KeypadEquals,
            Key.ShiftLeft => Input.ShiftLeft,
            Key.ControlLeft => Input.ControlLeft,
            Key.AltLeft => Input.AltLeft,
            Key.SuperLeft => Input.SuperLeft,
            Key.ShiftRight => Input.ShiftRight,
            Key.ControlRight => Input.ControlRight,
            Key.AltRight => Input.AltRight,
            Key.SuperRight => Input.SuperRight,
            Key.Menu => Input.Menu,
            _ => Input.Unknown,
        };
    }

    public static Input FromMouseButton(MouseButton button)
    {
        return button switch
        {
            MouseButton.Unknown => Input.Unknown,
            MouseButton.Left => Input.MouseLeft,
            MouseButton.Right => Input.MouseRight,
            MouseButton.Middle => Input.MouseMiddle,
            MouseButton.Button4 => Input.MouseButton4,
            MouseButton.Button5 => Input.MouseButton5,
            MouseButton.Button6 => Input.MouseButton6,
            MouseButton.Button7 => Input.MouseButton7,
            MouseButton.Button8 => Input.MouseButton8,
            MouseButton.Button9 => Input.MouseButton9,
            MouseButton.Button10 => Input.MouseButton10,
            MouseButton.Button11 => Input.MouseButton11,
            MouseButton.Button12 => Input.MouseButton12,
            _ => Input.Unknown,
        };
    }

    public static Input FromGamepadButton(Button button)
    {
        return button.Name switch
        {
            ButtonName.Unknown => Input.Unknown,
            ButtonName.A => Input.GamepadFaceDown,
            ButtonName.B => Input.GamepadFaceRight,
            ButtonName.X => Input.GamepadFaceLeft,
            ButtonName.Y => Input.GamepadFaceUp,
            ButtonName.LeftBumper => Input.GamepadBumperLeft,
            ButtonName.RightBumper => Input.GamepadBumperRight,
            ButtonName.Back => Input.GamepadBack,
            ButtonName.Start => Input.GamepadStart,
            ButtonName.Home => Input.GamepadHome,
            ButtonName.LeftStick => Input.GamepadStickLeft,
            ButtonName.RightStick => Input.GamepadStickRight,
            ButtonName.DPadUp => Input.GamepadDPadUp,
            ButtonName.DPadRight => Input.GamepadDPadRight,
            ButtonName.DPadDown => Input.GamepadDPadDown,
            ButtonName.DPadLeft => Input.GamepadDPadLeft,
            _ => Input.Unknown,
        };
    }
}