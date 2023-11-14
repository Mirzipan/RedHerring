using System.Numerics;
using RedHerring.Numbers;

namespace RedHerring.Studio.Models.ViewModels.Console;

public enum ConsoleItemType
{
	Info,
	Success,
	Warning,
	Error,
	Exception,
}

public static class ConsoleItemTypeExtensions
{
	public static Color4 ToColor(this ConsoleItemType type)
	{
		return type switch
		{
			ConsoleItemType.Info      => Color4.LightGray,
			ConsoleItemType.Success   => Color4.LightGreen,
			ConsoleItemType.Warning   => Color4.Gold,
			ConsoleItemType.Error     => Color4.Crimson,
			ConsoleItemType.Exception => Color4.MediumOrchid,
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
		};
	}
}