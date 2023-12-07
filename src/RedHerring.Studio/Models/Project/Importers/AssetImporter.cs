namespace RedHerring.Studio.Models.Project.Importers;

public abstract class AssetImporter<TSettings> : Importer where TSettings : ImporterSettings
{
	ImporterSettings Importer.CreateSettings() => CreateImporterSettings();
	ImporterResult Importer.Import(Stream stream, ImporterSettings settings, string resourcePath, CancellationToken cancellationToken) => Import(stream, (TSettings)settings, resourcePath, cancellationToken);

	protected abstract TSettings      CreateImporterSettings();
	protected abstract ImporterResult Import(Stream stream, TSettings settings, string resourcePath, CancellationToken cancellationToken);
}