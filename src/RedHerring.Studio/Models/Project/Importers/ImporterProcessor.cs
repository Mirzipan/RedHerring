namespace RedHerring.Studio.Models.Project.Importers;

public interface ImporterProcessor
{
	void Process(object? intermediate, ImporterSettings settings, string resourcePath);
}