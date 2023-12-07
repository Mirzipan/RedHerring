namespace RedHerring.Studio.Models.Project.Importers;

public interface Importer
{
    ImporterSettings CreateSettings();
    void Import(Stream stream, ImporterSettings settings, string resourcePath, CancellationToken cancellationToken);
}