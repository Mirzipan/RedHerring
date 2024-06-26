﻿using RedHerring.Alexandria.Identifiers;

namespace RedHerring.Inputs.Layers;

public class InputLayer
{
    private readonly Dictionary<string, ActionBinding> _bindings = new();
    
    public string? Name { get; set; }
    public StringId Layer { get; set; }
    public bool IsActive { get; internal set; }
    public bool ConsumesAllInput { get; set; }

    public event Action? Pushed;
    public event Action? Popped;

    #region Manipulation

    public void Bind(string action, ActionEventHandler handler) => Bind(action, InputState.Pressed, handler);

    public void Bind(string action, InputState state, ActionEventHandler handler)
    {
        _bindings[action] = new ActionBinding(state, handler);
    }

    public void Unbind(string action) => _bindings.Remove(action);

    public void UnbindAll() => _bindings.Clear();

    #endregion Manipulation

    #region Queries

    public bool HasBinding(string action) => _bindings.ContainsKey(action);

    public bool TryGetBinding(string action, out ActionBinding binding)
    {
        return _bindings.TryGetValue(action, out binding);
    }

    #endregion Queries

    #region Public

    public void Push()
    {
        var context = Interaction.CurrentContext();
        if (context is not null)
        {
            context.Layers.Push(this);
        }
    }

    public void Pop()
    {
        var context = Interaction.CurrentContext();
        if (context is not null)
        {
            context.Layers.Pop(this);
        }
    }

    #endregion Public

    #region Internal

    internal void RaisePushed() => Pushed?.Invoke();

    internal void RaisePopped() => Popped?.Invoke();

    #endregion Internal
}