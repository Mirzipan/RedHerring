namespace RedHerring.Studio.Models.Project.Importers;

public abstract class AssetImporterProcessor<TIntermediate, TSettings> : ImporterProcessor where TSettings : ImporterSettings
{
	void ImporterProcessor.Process(object? intermediate, ImporterSettings settings) => Process((TIntermediate?)intermediate, (TSettings)settings);

	protected abstract void Process(TIntermediate? intermediate, TSettings settings);
}