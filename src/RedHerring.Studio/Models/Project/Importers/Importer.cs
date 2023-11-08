namespace RedHerring.Studio.Models.Project.Importers;

public interface Importer
{
    object? Import(Stream stream, ImporterSettings settings);
}