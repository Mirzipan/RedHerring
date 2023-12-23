namespace EventAggregatorPlugin;

public class BroadcastEvent<TEvent> : BroadcastEventBase
{
	private readonly List<Action<TEvent>> _actions = new();
	
	public void AddListener(Action<TEvent> action)
	{
		_actions.Add(action);
	}

	public void RemoveListener(Action<TEvent> action)
	{
		_actions.Remove(action);
	}

	public void Invoke(TEvent ev)
	{
		for(int i=0; i < _actions.Count; ++i)
		{
			_actions[i].Invoke(ev);
		}
	}
}