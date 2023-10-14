namespace RedHerring.Fingerprint.Shortcuts;

public class AxisShortcut : IShortcut
{
    public AxisShortcut() : this (new CompositeShortcut(), new CompositeShortcut())
    {
    }

    public AxisShortcut(IShortcut? negativeShortcut, IShortcut? positiveShortcut)
    {
        NegativeShortcut = negativeShortcut;
        PositiveShortcut = positiveShortcut;
    }
    
    public IShortcut? NegativeShortcut { get; set; }
    public IShortcut? PositiveShortcut { get; set; }

    public void GetInputCodes(IList<InputCode> result)
    {
        NegativeShortcut?.GetInputCodes(result);
        PositiveShortcut?.GetInputCodes(result);
    }

    public float GetValue(Input input)
    {
        float negative = NegativeShortcut?.GetValue(input) ?? 0f;
        float positive = PositiveShortcut?.GetValue(input) ?? 0f;
        return positive - negative;
    }

    public bool IsUp(Input input) => false;

    public bool IsPressed(Input input) => false;

    public bool IsDown(Input input) => false;

    public bool IsReleased(Input input) => false;
}