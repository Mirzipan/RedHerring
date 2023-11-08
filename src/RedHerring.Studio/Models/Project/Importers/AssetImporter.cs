namespace RedHerring.Studio.Models.Project.Importers;

public abstract class AssetImporter<TIntermediate, TSettings> : Importer where TSettings : ImporterSettings
{
	object? Importer.Import(Stream stream, ImporterSettings settings) => Import(stream, (TSettings)settings);

	protected abstract TIntermediate? Import(Stream stream, TSettings settings);
}