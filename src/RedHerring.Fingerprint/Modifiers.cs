namespace RedHerring.Fingerprint;

[Flags]
public enum Modifiers
{
    None = 0,
    Alt = 1 << 0,
    Control = 1 << 1,
    Shift = 1 << 2,
    Meta = 1 << 3,
}