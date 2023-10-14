﻿using System.Collections.ObjectModel;

namespace RedHerring.Fingerprint.Shortcuts;

public class ShortcutBindings : Collection<ShortcutBinding>
{
    private readonly Dictionary<string, List<ShortcutBinding>> _actionsToShortcuts = new();
    private readonly Dictionary<IShortcut, HashSet<string>> _shortcutsToActions = new();

    public IReadOnlyCollection<string>? ActionsForShortcut(IShortcut shortcut)
    {
        return _shortcutsToActions.TryGetValue(shortcut, out var actions) ? actions : null;
    }

    protected override void InsertItem(int index, ShortcutBinding item)
    {
        Map(item);
        base.InsertItem(index, item);
    }

    protected override void SetItem(int index, ShortcutBinding item)
    {
        Map(item);
        base.SetItem(index, item);
    }

    protected override void RemoveItem(int index)
    {
        var item = this[index];
        Unmap(item);

        base.RemoveItem(index);
    }

    private void Map(ShortcutBinding item)
    {
        if (!_actionsToShortcuts.TryGetValue(item.Name!, out var bindings))
        {
            bindings = new List<ShortcutBinding>();
            _actionsToShortcuts[item.Name!] = bindings;
        }

        if (!_shortcutsToActions.TryGetValue(item.Shortcut!, out var actions))
        {
            actions = new HashSet<string>();
            _shortcutsToActions[item.Shortcut!] = actions;
        }
        
        bindings.Add(item);
        actions.Add(item.Name!);
    }

    private void Unmap(ShortcutBinding item)
    {
        if (_actionsToShortcuts.TryGetValue(item.Name!, out var bindings))
        {
            bindings.Remove(item);
        }

        if (_shortcutsToActions.TryGetValue(item.Shortcut!, out var actions))
        {
            actions.Remove(item.Name!);
        }
    }
}