namespace RedHerring.Studio.Models.Project.Importers;

public interface Importer
{
    ImporterSettings CreateSettings();
    object? Import(Stream stream, ImporterSettings settings);
}