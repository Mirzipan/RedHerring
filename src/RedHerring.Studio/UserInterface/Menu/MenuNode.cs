namespace RedHerring.Studio.UserInterface;

public abstract class AMenuNode
{
	public readonly string Name;
	
	protected AMenuNode(string name)
	{
		Name = name;
	}
	
	public abstract void Update();
}