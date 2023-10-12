using System.Runtime.InteropServices;

namespace RedHerring.Fingerprint.Shortcuts;

[StructLayout(LayoutKind.Explicit)]
public struct ShortcutValue
{
    [FieldOffset(0)]
    public int Id;
    [FieldOffset(0)]
    public Key Key;
    [FieldOffset(0)]
    public MouseButton MouseButton;
    [FieldOffset(0)]
    public MouseAxis MouseAxis;
    [FieldOffset(0)]
    public GamepadButton GamepadButton;
    [FieldOffset(0)]
    public GamepadAxis GamepadAxis;

    [FieldOffset(4)]
    public InputSource Source;

    public ShortcutValue(Key key)
    {
        Key = key;
        Source = InputSource.Keyboard;
    }

    public ShortcutValue(MouseButton button)
    {
        MouseButton = button;
        Source = InputSource.MouseButton;
    }

    public ShortcutValue(MouseAxis axis)
    {
        MouseAxis = axis;
        Source = InputSource.MouseAxis;
    }

    public ShortcutValue(GamepadButton button)
    {
        GamepadButton = button;
        Source = InputSource.GamepadButton;
    }

    public ShortcutValue(GamepadAxis axis)
    {
        GamepadAxis = axis;
        Source = InputSource.GamepadAxis;
    }
}