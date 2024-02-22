namespace RedHerring.Fingerprint.Shortcuts;

public sealed class GamepadAxisShortcut : Shortcut
{
    private InputCode _code;
    private Modifiers _modifiers;
    
    public GamepadAxis Axis => _code.GamepadAxis;
    public Modifiers Modifiers => _modifiers;

    public GamepadAxisShortcut(GamepadAxis axis, Modifiers modifiers = Modifiers.None)
    {
        _code = new InputCode(axis);
        _modifiers = modifiers;
    }

    public void InputCodes(IList<InputCode> result)
    {
        result.Add(_code);
    }

    public float Value(InteractionContext interactionContext) => interactionContext.AreModifiersDown(_modifiers) ? interactionContext.Axis(_code.GamepadAxis) : 0f;

    public bool IsPressed(InteractionContext interactionContext) => false;

    public bool IsDown(InteractionContext interactionContext) => Value(interactionContext) != 0;

    public bool IsReleased(InteractionContext interactionContext) => false;

    public override string ToString()
    {
        return $"Gamepad Axis: {_code.GamepadAxis}";
    }
}