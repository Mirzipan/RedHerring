namespace RedHerring.Fingerprint;

[Flags]
public enum InputState : byte
{
    Up = 0,
    Pressed = 1 << 0,
    Down = 1 << 1,
    Released = 1 << 2,
    Any = Pressed | Down | Released,
}