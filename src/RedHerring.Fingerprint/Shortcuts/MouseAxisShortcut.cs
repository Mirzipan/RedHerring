namespace RedHerring.Fingerprint.Shortcuts;

public sealed class MouseAxisShortcut : Shortcut
{
    private InputCode _code;
    private Modifiers _modifiers;
    
    public MouseAxis Axis => _code.MouseAxis;
    public Modifiers Modifiers => _modifiers;
    
    public MouseAxisShortcut(MouseAxis axis, Modifiers modifiers = Modifiers.None)
    {
        _code = new InputCode(axis);
        _modifiers = modifiers;
    }

    public void InputCodes(IList<InputCode> result) => result.Add(_code);

    public float Value(InteractionContext interactionContext) => interactionContext.AreModifiersDown(_modifiers) ? interactionContext.Axis(_code.MouseAxis) : 0f;

    public bool IsPressed(InteractionContext interactionContext) => false;

    public bool IsDown(InteractionContext interactionContext) => Value(interactionContext) != 0;

    public bool IsReleased(InteractionContext interactionContext) => false;

    public override string ToString()
    {
        return $"Mouse Axis: {_code.MouseAxis}";
    }
}