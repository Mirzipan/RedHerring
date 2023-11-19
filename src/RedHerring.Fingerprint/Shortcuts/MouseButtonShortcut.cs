namespace RedHerring.Fingerprint.Shortcuts;

public sealed class MouseButtonShortcut : Shortcut
{
    private InputCode _code;
    private Modifiers _modifiers;
    public MouseButton Button => _code.MouseButton;
    public Modifiers Modifiers => _modifiers;
    
    public MouseButtonShortcut(MouseButton button, Modifiers modifiers = Modifiers.None)
    {
        _code = new InputCode(button);
        _modifiers = modifiers;
    }

    public void InputCodes(IList<InputCode> result) => result.Add(_code);

    public float Value(Input input) => IsDown(input) ? 1f : 0f;

    public bool IsPressed(Input input) => input.AreModifiersDown(_modifiers) && input.IsButtonPressed(_code.MouseButton);

    public bool IsDown(Input input) => input.AreModifiersDown(_modifiers) && input.IsButtonDown(_code.MouseButton);

    public bool IsReleased(Input input) => input.AreModifiersDown(_modifiers) && input.IsButtonReleased(_code.MouseButton);

    public override string ToString()
    {
        return $"Mouse Button: {_code.MouseButton}";
    }
}