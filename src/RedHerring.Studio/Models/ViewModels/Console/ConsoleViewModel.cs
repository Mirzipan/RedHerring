namespace RedHerring.Studio.Models.ViewModels.Console;

public sealed class ConsoleViewModel
{
	private readonly List<ConsoleItem> _items = new();
	public IReadOnlyList<ConsoleItem> Items => _items;
	
	public void Log(string message, ConsoleItemType type)
	{
		_items.Add(new ConsoleItem(type, message));
	}
}