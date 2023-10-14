﻿namespace RedHerring.Fingerprint.States;

public class ActionsState : IActionState
{
    private readonly Dictionary<string, InputState> _states = new();

    public string Name { get; }
    public int Priority { get; set; }

    public ActionsState(string name)
    {
        Name = name;
    }

    public void Reset()
    {
        _states.Clear();
    }

    public void Add(string action, InputState state) => _states[action] = state;

    public bool IsActionUp(string action) => !_states.ContainsKey(action);

    public bool IsActionPressed(string action)
    {
        return _states.TryGetValue(action, out var state) && (state & InputState.Pressed) != 0;
    }

    public bool IsActionDown(string action)
    {
        return _states.TryGetValue(action, out var state) && (state & InputState.Down) != 0;
    }

    public bool IsActionReleased(string action)
    {
        return _states.TryGetValue(action, out var state) && (state & InputState.Released) != 0;
    }

    public bool IsAnyActionDown() => _states.Count > 0;

    public void GetActionsDown(IList<string> actions)
    {
        foreach (var pair in _states)
        {
            actions.Add(pair.Key);
        }
    }
}