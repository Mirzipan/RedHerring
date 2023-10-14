using System.Runtime.InteropServices;

namespace RedHerring.Fingerprint.Shortcuts;

[StructLayout(LayoutKind.Explicit)]
public struct InputCode
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

    public InputCode(Key key)
    {
        Key = key;
        Source = InputSource.Keyboard;
    }

    public InputCode(MouseButton button)
    {
        MouseButton = button;
        Source = InputSource.MouseButton;
    }

    public InputCode(MouseAxis axis)
    {
        MouseAxis = axis;
        Source = InputSource.MouseAxis;
    }

    public InputCode(GamepadButton button)
    {
        GamepadButton = button;
        Source = InputSource.GamepadButton;
    }

    public InputCode(GamepadAxis axis)
    {
        GamepadAxis = axis;
        Source = InputSource.GamepadAxis;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Source);
    }
}