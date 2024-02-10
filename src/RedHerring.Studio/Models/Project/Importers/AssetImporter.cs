using Migration;

namespace RedHerring.Studio.Models.Project.Importers;

public abstract class AssetImporter<TSettings> : Importer where TSettings : ImporterSettings
{
	ImporterSettings Importer.CreateSettings()                                                                                                                                                             => CreateImporterSettings();
	ImporterResult Importer.Import(
		Stream            stream, ImporterSettings settings,
		string            resourcePath,
		MigrationManager  migrationManager,
		CancellationToken cancellationToken,
		out string        referenceClassName
	) => Import(stream, (TSettings)settings, resourcePath, migrationManager, cancellationToken, out referenceClassName);

	protected abstract TSettings CreateImporterSettings();
	protected abstract ImporterResult Import(
		Stream            stream,
		TSettings         settings,
		string            resourcePath,
		MigrationManager  migrationManager,
		CancellationToken cancellationToken,
		out string        referenceClassName);
}