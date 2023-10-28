using RedHerring.Studio.Models;

namespace RedHerring.Studio.Tools;

public abstract class ATool
{
	protected readonly StudioModel StudioModel;

	private static     int _uniqueToolIdGenerator = 0;
	protected readonly int UniqueId               = _uniqueToolIdGenerator++;
	protected readonly string NameWithSalt;
	
	protected abstract string Name { get; }
	
	protected ATool(StudioModel studioModel)
	{
		NameWithSalt = $"{Name}##{UniqueId}";
		StudioModel = studioModel;
	}
	
	public abstract void Update(out bool finished);
}