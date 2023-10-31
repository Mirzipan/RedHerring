namespace RedHerring.Studio.Models.Project.Importers;

public abstract class AnImporter<T> : IImportAsset
{
	object? IImportAsset.Import(Stream stream) => Import(stream);

	public abstract T? Import(Stream stream);
}