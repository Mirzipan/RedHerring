using RedHerring.Studio.Models;

namespace RedHerring.Studio.Tools;

public abstract class ATool
{
	protected readonly StudioModel StudioModel;

	private static     int _uniqueToolIdGenerator = 0;
	protected readonly int UniqueId               = _uniqueToolIdGenerator++;
	
	protected ATool(StudioModel studioModel)
	{
		StudioModel = studioModel;
	}
	
	public abstract void Update(out bool finished);
}