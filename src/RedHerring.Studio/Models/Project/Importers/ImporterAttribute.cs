using RedHerring.Studio.Models.Project.FileSystem;

namespace RedHerring.Studio;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ImporterAttribute : Attribute
{
	public readonly ProjectNodeType NodeType; 
	
	public ImporterAttribute(ProjectNodeType nodeType = ProjectNodeType.Uninitialized)
	{
		NodeType   = nodeType;
	}
}