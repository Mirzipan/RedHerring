namespace RedHerring.Fingerprint;

public readonly struct Shortcut
{
    public readonly Input Positive;
    public readonly Input Negative;
    public readonly Modifier Modifiers;

    public Shortcut(Input input, Modifier modifiers = Modifier.None) : this(input, Input.Unknown, modifiers)
    {
    }

    public Shortcut(Input positive, Input negative, Modifier modifiers = Modifier.None)
    {
        Positive = positive;
        Negative = negative;
        Modifiers = modifiers;
    }
}