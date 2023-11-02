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

    public float Value(Input input)
    {
        float negative = NegativeShortcut?.Value(input) ?? 0f;
        float positive = PositiveShortcut?.Value(input) ?? 0f;
        return positive - negative;
    }

    public bool IsUp(Input input) => false;

    public bool IsPressed(Input input) => false;

    public bool IsDown(Input input) => false;

    public bool IsReleased(Input input) => false;
}