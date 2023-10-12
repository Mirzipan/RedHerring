namespace RedHerring.Fingerprint.Shortcuts;

public abstract class AShortcut : IShortcut
{
    protected readonly ShortcutValue Value;

    public InputSource Source => Value.Source;
    public int Id => Value.Id;

    protected AShortcut(InputSource source, int id)
    {
        Value = new ShortcutValue
        {
            Source = source,
            Id = id,
        };
    }

    public abstract float GetValue(Input input);

    public abstract bool IsUp(Input input);

    public abstract bool IsPressed(Input input);

    public abstract bool IsDown(Input input);

    public abstract bool IsReleased(Input input);
}