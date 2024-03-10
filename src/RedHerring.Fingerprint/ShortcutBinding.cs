namespace RedHerring.Fingerprint;

public class ShortcutBinding
{
    public ShortcutBinding(string name, Shortcut shortcut)
    {
        Name = name;
        Shortcut = shortcut;
    }

    public string? Name { get; set; }
    public Shortcut Shortcut { get; set; }
}