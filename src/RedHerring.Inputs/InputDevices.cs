namespace RedHerring.Inputs;

internal interface InputDevices
{
    void NextFrame();
    event Action<InputChanged>? InputChanged;
    event Action<int, char>? CharacterTyped;
    void Cursor(CursorKind cursor);
}