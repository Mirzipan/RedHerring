namespace RedHerring.Fingerprint.Shortcuts;

public class ShortcutBinding
{
    public ShortcutBinding() : this(null)
    {
    }

    public ShortcutBinding(string? name, IShortcut? shortcut = null)
    {
        Name = name;
        Shortcut = shortcut;
    }

    public string? Name { get; set; }
    public IShortcut? Shortcut { get; set; }
}