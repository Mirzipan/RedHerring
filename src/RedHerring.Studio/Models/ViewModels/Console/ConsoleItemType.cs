using System.Numerics;

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
	public static Vector4 ToColor(this ConsoleItemType type)
	{
		return type switch
		{
			ConsoleItemType.Info      => new(0.8f, 0.8f, 0.8f, 1.0f),
			ConsoleItemType.Success   => new(0.0f, 1.0f, 0.0f, 1.0f),
			ConsoleItemType.Warning   => new(1.0f, 0.5f, 0.0f, 1.0f),
			ConsoleItemType.Error     => new(1.0f, 0.0f, 0.0f, 1.0f),
			ConsoleItemType.Exception => new(1.0f, 0.0f, 0.5f, 1.0f),
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
		};
	}
}