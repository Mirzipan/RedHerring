namespace RedHerring.Studio.UserInterface;

public abstract class MenuNode
{
	public readonly string Name;
	
	protected MenuNode(string name)
	{
		Name = name;
	}
	
	public abstract void Update();
}