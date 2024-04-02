namespace RedHerring.Inputs;

public sealed class NullDevices : InputDevices
{
    public event Action<InputChanged>? InputChanged;
    public event Action<int, char>? CharacterTyped;

    public NullDevices()
    {
    }

    public void NextFrame()
    {
    }

    public void Cursor(CursorKind cursor)
    {
    }

    public string Clipboard() => string.Empty;

    public void Clipboard(string text)
    {
    }
}