namespace RedHerring.Studio.Tools;

public readonly struct ToolId
{
	public readonly string Name;
	public readonly int    UniqueId;

	public ToolId(string name, int uniqueId)
	{
		Name     = name;
		UniqueId = uniqueId;
	}
}