namespace EventAggregatorPlugin;

public interface IGenericEventAggregatorReadOnly<TBaseEvent> where TBaseEvent : IBaseEvent
{
	void Register<TEvent>(Action<TEvent>   action) where TEvent : TBaseEvent;
	void Unregister<TEvent>(Action<TEvent> action) where TEvent : TBaseEvent;

	void Register<TEvent>(object   key, Action<TEvent> action) where TEvent : TBaseEvent;
	void Unregister<TEvent>(object key, Action<TEvent> action) where TEvent : TBaseEvent;
}