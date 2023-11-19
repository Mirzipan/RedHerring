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

    public float Value(Input input) => input.AreModifiersDown(_modifiers) ? input.Axis(_code.MouseAxis) : 0f;

    public bool IsPressed(Input input) => false;

    public bool IsDown(Input input) => Value(input) != 0;

    public bool IsReleased(Input input) => false;

    public override string ToString()
    {
        return $"Mouse Axis: {_code.MouseAxis}";
    }
}