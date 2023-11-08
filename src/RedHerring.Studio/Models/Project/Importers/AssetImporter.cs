namespace RedHerring.Studio.Models.Project.Importers;

public abstract class AssetImporter<T_INTERMEDIATE, T_SETTINGS> : Importer where T_SETTINGS : ImporterSettings
{
	object? Importer.Import(Stream stream, ImporterSettings settings) => Import(stream, (T_SETTINGS)settings);

	public abstract T_INTERMEDIATE? Import(Stream stream, T_SETTINGS settings);
}