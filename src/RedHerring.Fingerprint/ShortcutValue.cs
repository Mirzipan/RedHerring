using Silk.NET.Input;
using Silk.NET.SDL;

namespace RedHerring.Fingerprint;

public struct ShortcutValue
{
    public byte Value;

    public Key Key => (Key)Value;
    public MouseButton MouseButton => (MouseButton)Value;
    public GameControllerButton ControllerButton => (GameControllerButton)Value;
    public GameControllerAxis ControllerAxis => (GameControllerAxis)Value;
}