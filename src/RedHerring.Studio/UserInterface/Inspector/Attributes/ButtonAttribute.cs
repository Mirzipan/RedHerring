namespace RedHerring.Studio.UserInterface.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class ButtonAttribute : Attribute
{
	public readonly string? Title;

	public ButtonAttribute(string? title = null)
	{
		Title = title;
	}
}