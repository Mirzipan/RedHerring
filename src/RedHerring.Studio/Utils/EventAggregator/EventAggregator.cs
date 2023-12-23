namespace EventAggregatorPlugin;

// simple implementation, if nothing more is needed
public class EventAggregator : GenericEventAggregator<IBaseEvent>, IEventAggregatorReadOnly
{
}