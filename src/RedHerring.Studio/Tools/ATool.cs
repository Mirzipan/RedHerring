using RedHerring.Studio.Models;

namespace RedHerring.Studio.Tools;

public abstract class ATool
{
	protected readonly StudioModel StudioModel;

	private static int _uniqueToolIdGenerator = 0;
	public static  int UniqueToolIdGeneratorState => _uniqueToolIdGenerator; // used for saving
	
	private readonly   int    UniqueId;
	protected readonly string NameWithSalt;
	
	protected abstract string Name { get; }
	public ToolId Id => new(Name, UniqueId);
	
	protected ATool(StudioModel studioModel)
		: this(studioModel, _uniqueToolIdGenerator++)
	{
	}

	protected ATool(StudioModel studioModel, int uniqueId)
	{
		UniqueId     = uniqueId;
		NameWithSalt = $"{Name}##{UniqueId}";
		StudioModel  = studioModel;
	}
	
	public static void SetUniqueIdGenerator(int value) // used for loading
	{
		_uniqueToolIdGenerator = value;
	}
	
	public abstract void Update(out bool finished);
}