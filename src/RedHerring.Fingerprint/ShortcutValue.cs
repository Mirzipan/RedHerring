using System.Runtime.InteropServices;

namespace RedHerring.Fingerprint;

[StructLayout(LayoutKind.Explicit)]
public struct ShortcutValue
{
    [FieldOffset(0)]
    public int Value;
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
}