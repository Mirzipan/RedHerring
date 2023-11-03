namespace RedHerring.Studio.Models;

// TODO - temporary, just for now
public sealed class StudioTheme
{
	public readonly string Name;
	public readonly Action Apply;

	public StudioTheme(string name, Action apply)
	{
		Name  = name;
		Apply = apply;
	}
	
	public override string ToString() => Name;
}