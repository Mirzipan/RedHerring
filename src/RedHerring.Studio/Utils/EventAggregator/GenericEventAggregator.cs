namespace EventAggregatorPlugin;

// generic event aggregator
public class GenericEventAggregator<TBaseEvent> : IGenericEventAggregatorReadOnly<TBaseEvent> where TBaseEvent : IBaseEvent
{
	//------ Types ------
	private class GenericUnityEvent<TEvent> : BroadcastEvent<TEvent> where TEvent : TBaseEvent {}

	//------ Attributes ------
	private readonly Dictionary<Type, BroadcastEventBase> _broadcastHandlers = new();
	private readonly Dictionary<Type, HashedEventBase>    _hashedHandlers    = new();

	//------ Methods ------
	#region Broadcast
	public void Trigger<TEvent>(TEvent event_instance) where TEvent : TBaseEvent
	{
		TriggerAllHashedInternal(event_instance); // also all hashed
		TriggerBroadcastInternal(event_instance);
	}

	public void Register<TEvent>(Action<TEvent> action) where TEvent : TBaseEvent
	{
		if (!_broadcastHandlers.ContainsKey(typeof(TEvent)))
		{
			_broadcastHandlers.Add(typeof(TEvent), new GenericUnityEvent<TEvent>());
		}

		((BroadcastEvent<TEvent>)_broadcastHandlers[typeof(TEvent)]).AddListener(action);
	}

	public void Unregister<TEvent>(Action<TEvent> action) where TEvent : TBaseEvent
	{
		if (!_broadcastHandlers.ContainsKey(typeof(TEvent)))
		{
			return;
		}

		((BroadcastEvent<TEvent>)_broadcastHandlers[typeof(TEvent)]).RemoveListener(action);
	}

	public void TriggerBroadcastInternal<TEvent>(TEvent event_instance) where TEvent : TBaseEvent
	{
		if (!_broadcastHandlers.TryGetValue(typeof(TEvent), out BroadcastEventBase handler))
		{
			return;
		}

		((BroadcastEvent<TEvent>)handler).Invoke(event_instance);
	}
	#endregion
        
	//---------------------
	#region Hashed
	public void Trigger<TEvent>(object key, TEvent event_instance) where TEvent : TBaseEvent
	{
		TriggerBroadcastInternal(event_instance); // also broadcast
		TriggerHashedInternal(key, event_instance);
	}

	public void Register<TEvent>(object key, Action<TEvent> action) where TEvent : TBaseEvent
	{
		if (!_hashedHandlers.ContainsKey(typeof(TEvent)))
		{
			_hashedHandlers.Add(typeof(TEvent), new HashedEvent<TEvent>());
		}

		((HashedEvent<TEvent>)_hashedHandlers[typeof(TEvent)]).AddUniqueListener(key.GetHashCode(), action);
	}

	public void Unregister<TEvent>(object key, Action<TEvent> action) where TEvent : TBaseEvent
	{
		if (!_hashedHandlers.ContainsKey(typeof(TEvent)))
		{
			return;
		}

		((HashedEvent<TEvent>)_hashedHandlers[typeof(TEvent)]).RemoveListener(key.GetHashCode(), action);
	}

	private void TriggerHashedInternal<TEvent>(object key, TEvent event_instance) where TEvent : TBaseEvent
	{        
		if (!_hashedHandlers.TryGetValue(typeof(TEvent), out HashedEventBase handler))
		{
			return;
		}

		((HashedEvent<TEvent>)handler).Invoke(key.GetHashCode(), event_instance);
	}
        
	private void TriggerAllHashedInternal<TEvent>(TEvent event_instance) where TEvent : TBaseEvent
	{
		if (!_hashedHandlers.TryGetValue(typeof(TEvent), out HashedEventBase handler))
		{
			return;
		}

		((HashedEvent<TEvent>)handler).InvokeAll(event_instance);
	}
	#endregion
}