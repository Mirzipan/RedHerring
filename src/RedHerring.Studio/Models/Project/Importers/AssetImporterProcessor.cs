namespace RedHerring.Studio.Models.Project.Importers;

public abstract class AssetImporterProcessor<TIntermediate, TSettings> : ImporterProcessor where TSettings : ImporterSettings
{
	void ImporterProcessor.Process(object? intermediate, ImporterSettings settings, string resourcePath) => Process((TIntermediate?)intermediate, (TSettings)settings, resourcePath);

	protected abstract void Process(TIntermediate? intermediate, TSettings settings, string resourcePath);
}