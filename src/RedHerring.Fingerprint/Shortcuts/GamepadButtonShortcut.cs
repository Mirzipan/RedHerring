namespace RedHerring.Fingerprint.Shortcuts;

public sealed class GamepadButtonShortcut : Shortcut
{
    private InputCode _code;
    private Modifiers _modifiers;
    
    public GamepadButton Button => _code.GamepadButton;
    public Modifiers Modifiers => _modifiers;
    
    
    public GamepadButtonShortcut(GamepadButton button, Modifiers modifiers = Modifiers.None)
    {
        _code = new InputCode(button);
        _modifiers = modifiers;
    }

    public void InputCodes(IList<InputCode> result) => result.Add(_code);

    public float Value(Input input) => IsDown(input) ? 1f : 0f;

    public bool IsPressed(Input input) => input.IsButtonPressed(_code.GamepadButton);

    public bool IsDown(Input input) => input.IsButtonDown(_code.GamepadButton);

    public bool IsReleased(Input input) => input.IsButtonReleased(_code.GamepadButton);

    public override string ToString()
    {
        return $"Gamepad Button: {_code.GamepadButton}";
    }
}