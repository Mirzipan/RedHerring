namespace RedHerring.Inputs;

public interface InputDevices
{
    void NextFrame();
    event Action<InputChanged>? InputChanged;
    event Action<int, char>? CharacterTyped;
    void Cursor(CursorKind cursor);
    string Clipboard();
    void Clipboard(string text);
}