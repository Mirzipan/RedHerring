namespace RedHerring.Fingerprint.States;

public class ActionsState
{
    private readonly Dictionary<string, InputState> _states = new();

    internal ActionsState()
    {
    }
    
    public void Reset()
    {
        _states.Clear();
    }

    public void Add(string action, InputState state) => _states[action] = state;

    public bool IsUp(string action) => !_states.ContainsKey(action);

    public bool IsPressed(string action)
    {
        return _states.TryGetValue(action, out var state) && (state & InputState.Pressed) != 0;
    }

    public bool IsDown(string action)
    {
        return _states.TryGetValue(action, out var state) && (state & InputState.Down) != 0;
    }

    public bool IsReleased(string action)
    {
        return _states.TryGetValue(action, out var state) && (state & InputState.Released) != 0;
    }

    public bool Any() => _states.Count > 0;

    public void ActionsDown(IList<string> actions)
    {
        foreach (var pair in _states)
        {
            actions.Add(pair.Key);
        }
    }
}