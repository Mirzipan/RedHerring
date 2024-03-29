namespace RedHerring.Studio.Models.ViewModels.Console;

public sealed class ConsoleViewModel
{
	private static ConsoleViewModel _this = null!;
	
	private readonly List<ConsoleItem> _items = new();

	public ConsoleItem this[int index]
	{
		get
		{
			lock (_items)
			{
				return _items[index];
			}
		}
	}
	
	public int Count
	{
		get
		{
			lock (_items)
			{
				return _items.Count;
			}
		}
	}

	public ConsoleViewModel()
	{
		_this = this;
	}
	
	public static void Log(string? message, ConsoleItemKind kind)
	{
		lock (_this._items)
		{
			_this._items.Add(new ConsoleItem(kind, message));
		}
	}

	public static void LogInfo(string?      message) => Log(message, ConsoleItemKind.Info);
	public static void LogWarning(string?   message) => Log(message, ConsoleItemKind.Warning);
	public static void LogError(string?     message) => Log(message, ConsoleItemKind.Error);
	public static void LogException(string? message) => Log(message, ConsoleItemKind.Exception);
}