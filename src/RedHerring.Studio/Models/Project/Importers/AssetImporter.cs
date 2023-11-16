namespace RedHerring.Studio.Models.Project.Importers;

public abstract class AssetImporter<TIntermediate, TSettings> : Importer where TSettings : ImporterSettings
{
	ImporterSettings Importer.CreateSettings()                                 => CreateImporterSettings();
	object? Importer.         Import(Stream stream, ImporterSettings settings) => Import(stream, (TSettings)settings);

	protected abstract TSettings      CreateImporterSettings();
	protected abstract TIntermediate? Import(Stream stream, TSettings settings);
}