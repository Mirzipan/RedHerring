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

    public float Value(InteractionContext interactionContext) => IsDown(interactionContext) ? 1f : 0f;

    public bool IsPressed(InteractionContext interactionContext) => interactionContext.IsButtonPressed(_code.GamepadButton);

    public bool IsDown(InteractionContext interactionContext) => interactionContext.IsButtonDown(_code.GamepadButton);

    public bool IsReleased(InteractionContext interactionContext) => interactionContext.IsButtonReleased(_code.GamepadButton);

    public override string ToString()
    {
        return $"Gamepad Button: {_code.GamepadButton}";
    }
}