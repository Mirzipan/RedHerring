using RedHerring.Studio.Models.Project.FileSystem;

namespace RedHerring.Studio;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ImporterAttribute : Attribute
{
	public readonly ProjectNodeKind NodeKind; 
	
	public ImporterAttribute(ProjectNodeKind nodeKind = ProjectNodeKind.Uninitialized)
	{
		NodeKind   = nodeKind;
	}
}