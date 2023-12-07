namespace RedHerring.Studio.Models.Project.FileSystem;

[Flags]
public enum TraverseFlags
{
	Directories = 0x01,
	Files       = 0x02,
}