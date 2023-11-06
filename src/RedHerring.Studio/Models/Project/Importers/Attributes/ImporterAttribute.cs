namespace RedHerring.Studio.Models.Project.Importers;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ImporterAttribute : System.Attribute
{
	public readonly Type?    RunsAfter;
	public readonly string[] Extensions;
	
	public ImporterAttribute(params string[] extensions)
	{
		RunsAfter  = null;
		Extensions = extensions;
	}

	public ImporterAttribute(Type runsAfter, params string[] extensions)
	{
		RunsAfter  = runsAfter;
		Extensions = extensions;
	}
}