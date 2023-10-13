using System.Collections.ObjectModel;

namespace RedHerring.Fingerprint.Shortcuts;

public class ShortcutBindings : Collection<ShortcutBinding>
{
    private readonly Dictionary<string, List<ShortcutBinding>> _namesToShortcuts = new();

    public IReadOnlyCollection<string> Actions => _namesToShortcuts.Keys;

    protected override void InsertItem(int index, ShortcutBinding item)
    {
        if (!_namesToShortcuts.TryGetValue(item.Name!, out var values))
        {
            values = new List<ShortcutBinding>();
            _namesToShortcuts[item.Name!] = values;
        }
        
        
        values.Add(item);
        base.InsertItem(index, item);
    }

    protected override void SetItem(int index, ShortcutBinding item)
    {
        if (!_namesToShortcuts.TryGetValue(item.Name!, out var values))
        {
            values = new List<ShortcutBinding>();
            _namesToShortcuts[item.Name!] = values;
        }
        
        values.Add(item);
        base.SetItem(index, item);
    }

    protected override void RemoveItem(int index)
    {
        var item = this[index];
        if (_namesToShortcuts.TryGetValue(item.Name!, out var values))
        {
            values.Remove(item);
        }
        
        base.RemoveItem(index);
    }

    public float GetValue(Input input, string name)
    {
        float result = 0;
        if (!_namesToShortcuts.TryGetValue(name, out var bindings))
        {
            return result;
        }

        foreach (var entry in bindings)
        {
            float value = entry.GetValue(input);
            if (Math.Abs(value) > Math.Abs(result))
            {
                result = value;
            }
        }

        return result;
    }
}