﻿namespace RedHerring.Fingerprint.Shortcuts;

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

    public IEnumerable<ShortcutValue> InputValues()
    {
        if (NegativeShortcut is not null)
        {
            foreach (var value in NegativeShortcut.InputValues())
            {
                yield return value;
            }
        }

        if (PositiveShortcut is not null)
        {
            foreach (var value in PositiveShortcut.InputValues())
            {
                yield return value;
            }
        }
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