namespace RedHerring.Studio.Models.Project.Importers;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ImporterAttribute : System.Attribute
{
	public readonly string[] Extensions;
	
	public ImporterAttribute(params string[] extensions)
	{
		Extensions = extensions;
	}
}