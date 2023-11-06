namespace RedHerring.Studio.Models.Project.Importers;

public abstract class AssetImporter<T> : Importer
{
	object? Importer.Import(Stream stream) => Import(stream);

	public abstract T? Import(Stream stream);
}