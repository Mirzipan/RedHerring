namespace RedHerring.Fingerprint.Shortcuts;

public abstract class AShortcut : IShortcut
{
    protected readonly InputCode Code;

    public InputSource Source => Code.Source;
    public int Id => Code.Id;

    protected AShortcut(InputSource source, int id)
    {
        Code = new InputCode
        {
            Source = source,
            Id = id,
        };
    }

    public void GetInputCodes(IList<InputCode> result)
    {
        result.Add(Code);
    }

    public abstract float GetValue(Input input);

    public abstract bool IsUp(Input input);

    public abstract bool IsPressed(Input input);

    public abstract bool IsDown(Input input);

    public abstract bool IsReleased(Input input);
}