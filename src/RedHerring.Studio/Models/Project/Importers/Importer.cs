using RedHerring.Studio.Models.Project.FileSystem;

namespace RedHerring.Studio;

public abstract class Importer
{
	protected readonly ProjectNode Owner;
	public abstract    string      ReferenceType { get; }

	protected Importer(ProjectNode owner)
	{
		Owner = owner;
	}

	public abstract void UpdateCache();
	public abstract void ClearCache();

	public abstract void Import(string resourcesRootPath, out string? relativeResourcePath);

	public abstract ImporterSettings CreateImportSettings();
	public abstract bool             UpdateImportSettings(ImporterSettings settings); // returns true if settings were changed
}

public abstract class Importer<TData> : Importer
{
	protected Importer(ProjectNode owner) : base(owner)
	{
	}

	public abstract TData? Load(); 
	public abstract void   Save(TData                          data);
}