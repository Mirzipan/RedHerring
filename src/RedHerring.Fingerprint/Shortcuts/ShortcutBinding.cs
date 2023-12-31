﻿namespace RedHerring.Fingerprint.Shortcuts;

public class ShortcutBinding
{
    public ShortcutBinding() : this(null)
    {
    }

    public ShortcutBinding(string? name, Shortcut? shortcut = null)
    {
        Name = name;
        Shortcut = shortcut;
    }

    public string? Name { get; set; }
    public Shortcut? Shortcut { get; set; }

    public float Value(Input input) => Shortcut?.Value(input) ?? 0f;
}