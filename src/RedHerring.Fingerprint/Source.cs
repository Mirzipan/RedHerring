namespace RedHerring.Fingerprint;

[Flags]
public enum Source
{
    None = 0,
    Keyboard = 1 << 0,
    MouseButton = 1 << 1,
    MouseAxis = 1 << 2,
    GamepadButton = 1 << 3,
    GamepadAxis = 1 << 4,
}