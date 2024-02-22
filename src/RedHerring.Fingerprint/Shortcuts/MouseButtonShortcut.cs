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

    public float Value(InteractionContext interactionContext) => IsDown(interactionContext) ? 1f : 0f;

    public bool IsPressed(InteractionContext interactionContext) => interactionContext.AreModifiersDown(_modifiers) && interactionContext.IsButtonPressed(_code.MouseButton);

    public bool IsDown(InteractionContext interactionContext) => interactionContext.AreModifiersDown(_modifiers) && interactionContext.IsButtonDown(_code.MouseButton);

    public bool IsReleased(InteractionContext interactionContext) => interactionContext.AreModifiersDown(_modifiers) && interactionContext.IsButtonReleased(_code.MouseButton);

    public override string ToString()
    {
        return $"Mouse Button: {_code.MouseButton}";
    }
}