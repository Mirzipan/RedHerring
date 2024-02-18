using EventAggregatorPlugin;

namespace RedHerring.Studio.Models;

public sealed class StudioModelEventAggregator : GenericEventAggregator<IStudioModelEvent>, IStudioModelEventAggregatorReadOnly
{
}