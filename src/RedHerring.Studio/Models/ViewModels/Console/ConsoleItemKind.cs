using RedHerring.Numbers;

namespace RedHerring.Studio.Models.ViewModels.Console;

public enum ConsoleItemKind
{
	Info,
	Success,
	Warning,
	Error,
	Exception,
}

public static class ConsoleItemTypeExtensions
{
	public static Color4 ToColor(this ConsoleItemKind type)
	{
		return type switch
		{
			ConsoleItemKind.Info      => Color4.LightGray,
			ConsoleItemKind.Success   => Color4.LightGreen,
			ConsoleItemKind.Warning   => Color4.Gold,
			ConsoleItemKind.Error     => Color4.Crimson,
			ConsoleItemKind.Exception => Color4.MediumOrchid,
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
		};
	}
}