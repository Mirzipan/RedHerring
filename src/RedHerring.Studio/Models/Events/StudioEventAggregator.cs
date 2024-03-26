using EventAggregatorPlugin;

namespace RedHerring.Studio.Models;

public sealed class StudioEventAggregator : GenericEventAggregator<StudioEvent>, IGenericEventAggregatorReadOnly<StudioEvent>
{
}