namespace RedHerring.Fingerprint.Shortcuts;

public class AxisShortcut : Shortcut
{
    public AxisShortcut() : this (new CompositeShortcut(), new CompositeShortcut())
    {
    }

    public AxisShortcut(Shortcut? negativeShortcut, Shortcut? positiveShortcut)
    {
        NegativeShortcut = negativeShortcut;
        PositiveShortcut = positiveShortcut;
    }
    
    public Shortcut? NegativeShortcut { get; set; }
    public Shortcut? PositiveShortcut { get; set; }

    public void InputCodes(IList<InputCode> result)
    {
        NegativeShortcut?.InputCodes(result);
        PositiveShortcut?.InputCodes(result);
    }

    public float Value(InteractionContext interactionContext)
    {
        float negative = NegativeShortcut?.Value(interactionContext) ?? 0f;
        float positive = PositiveShortcut?.Value(interactionContext) ?? 0f;
        return positive - negative;
    }

    public bool IsPressed(InteractionContext interactionContext) => false;

    public bool IsDown(InteractionContext interactionContext) => false;

    public bool IsReleased(InteractionContext interactionContext) => false;
}