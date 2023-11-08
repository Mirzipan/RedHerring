namespace RedHerring.Studio.Models.Project.Importers;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ImporterProcessorAttribute : Attribute
{
	public readonly Type ProcessedType;
	
	public ImporterProcessorAttribute(Type processedType)
	{
		ProcessedType = processedType;
	}
}