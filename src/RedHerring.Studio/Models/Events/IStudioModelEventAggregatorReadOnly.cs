using EventAggregatorPlugin;

namespace RedHerring.Studio.Models;

public interface IStudioModelEventAggregatorReadOnly : IGenericEventAggregatorReadOnly<IStudioModelEvent>
{
	
}