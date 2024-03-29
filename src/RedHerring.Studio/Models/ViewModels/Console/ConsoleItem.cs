namespace RedHerring.Studio.Models.ViewModels.Console;

public readonly struct ConsoleItem
{
	public readonly ConsoleItemKind Kind;
	public readonly DateTime TimeStamp;
	public readonly string          Message;

	public ConsoleItem(ConsoleItemKind kind, string? message)
	{
		Kind    = kind;
		TimeStamp = DateTime.Now;
		Message = message ?? "<null>";
	}
}