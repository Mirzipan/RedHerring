namespace RedHerring.Fingerprint.Shortcuts;

public sealed class KeyboardShortcut : Shortcut
{
    private InputCode _code;
    public Key Key => _code.Key;
    
    public KeyboardShortcut(Key key) => _code = new InputCode(key);

    public void InputCodes(IList<InputCode> result) => result.Add(_code);

    public float Value(Input input) => IsDown(input) ? 1f : 0f;

    public bool IsUp(Input input) => input.IsKeyUp(_code.Key);

    public bool IsPressed(Input input) => input.IsKeyPressed(_code.Key);

    public bool IsDown(Input input) => input.IsKeyDown(_code.Key);

    public bool IsReleased(Input input) => input.IsKeyReleased(_code.Key);
}