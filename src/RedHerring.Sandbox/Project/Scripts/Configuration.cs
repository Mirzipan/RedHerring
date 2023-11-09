using RedHerring.Core;
using RedHerring.Fingerprint;
using RedHerring.Fingerprint.Shortcuts;
using RedHerring.Infusion.Attributes;

namespace RedHerring.Sandbox;

public sealed class Configuration : EngineSystem
{
    [Infuse]
    private Input _input = null!;
    
    protected override void Init()
    {
        Input();
    }

    private void Input()
    {
        if (_input.Bindings is null)
        {
            _input.Bindings = new ShortcutBindings();
        }
        
        _input.Bindings.Add(new ShortcutBinding("toggle_menu", new KeyboardShortcut(Key.Escape)));
    }
}