namespace EventAggregatorPlugin;

public class HashedEvent<TEvent> : HashedEventBase
{
	private readonly List<KeyValuePair<int, Action<TEvent>>> _actions = new();

	private class ActionsComparer : IComparer<KeyValuePair<int, Action<TEvent>>>
	{
		public int Compare(KeyValuePair<int, Action<TEvent>> x, KeyValuePair<int, Action<TEvent>> y)
		{
			return x.Key - y.Key; //x.Key.CompareTo(y.Key);
		}
	}

	private readonly ActionsComparer m_Comparer = new ();

	public void AddUniqueListener(int hash, Action<TEvent> action)
	{
		KeyValuePair<int, Action<TEvent>> pair  = new KeyValuePair<int, Action<TEvent>>(hash, action);
		int                               index = _actions.BinarySearch(pair, m_Comparer);
		if (index < 0)
		{
			_actions.Insert(~index, pair);
		}
		else
		{
			int first_hash_index = FindFirstIndex(index, hash);
			int action_index     = FindAction(first_hash_index, hash, action);
			if (action_index >= 0)
			{
				return;
			}

			_actions.Insert(index, pair);
		}
	}

	public void RemoveListener(int hash, Action<TEvent> action)
	{
		KeyValuePair<int, Action<TEvent>> pair  = new KeyValuePair<int, Action<TEvent>>(hash, action);
		int                               index = _actions.BinarySearch(pair, m_Comparer);
		if (index < 0)
		{
			return;
		}

		index = FindFirstIndex(index, hash);
		index = FindAction(index, hash, action);
		if (index >= 0)
		{
			_actions.RemoveAt(index);
		}
	}

	public void Invoke(int hash, TEvent event_instance)
	{
		KeyValuePair<int, Action<TEvent>> pair  = new KeyValuePair<int, Action<TEvent>>(hash, null);
		int                               index = _actions.BinarySearch(pair, m_Comparer);
		if (index < 0)
		{
			return;
		}

		index = FindFirstIndex(index, hash);
		for (int i = index; i < _actions.Count && _actions[i].Key == hash; ++i)
		{
			_actions[i].Value.Invoke(event_instance);
		}
	}

	public void InvokeAll(TEvent event_instance)
	{
		for (int i = 0; i < _actions.Count; ++i) // DON'T USE FOREACH, COLLECTION MAY CHANGE!
		{
			_actions[i].Value.Invoke(event_instance);
		}
	}

	private int FindFirstIndex(int index_from, int hash)
	{
		while (index_from >= 0 && _actions[index_from].Key == hash)
		{
			--index_from;
		}

		return ++index_from;
	}

	private int FindAction(int index_from, int hash, Action<TEvent> action)
	{
		for (int i = index_from; i < _actions.Count && _actions[i].Key == hash; ++i)
		{
			//if (ReferenceEquals(m_Actions[i].Value, action))
			if(_actions[i].Value == action)
			{
				return i;
			}
		}

		return -1;
	}
}