namespace RedHerring.Fingerprint.Shortcuts;

public sealed class KeyboardShortcut : Shortcut
{
    private InputCode _code;
    private Modifiers _modifiers;
    
    public Key Key => _code.Key;
    public Modifiers Modifiers => _modifiers;
    
    public KeyboardShortcut(Key key, Modifiers modifiers = Modifiers.None)
    {
        _code = new InputCode(key);
        _modifiers = modifiers;
    }

    public void InputCodes(IList<InputCode> result) => result.Add(_code);

    public float Value(InteractionContext interactionContext) => IsDown(interactionContext) ? 1f : 0f;

    public bool IsPressed(InteractionContext interactionContext) => interactionContext.AreModifiersDown(_modifiers) && interactionContext.IsKeyPressed(_code.Key);

    public bool IsDown(InteractionContext interactionContext) => interactionContext.AreModifiersDown(_modifiers) && interactionContext.IsKeyDown(_code.Key);

    public bool IsReleased(InteractionContext interactionContext) => interactionContext.AreModifiersDown(_modifiers) && interactionContext.IsKeyReleased(_code.Key);

    public override string ToString()
    {
        return $"Key: {_code.Key}";
    }
}