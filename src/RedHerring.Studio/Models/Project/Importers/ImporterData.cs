using RedHerring.Studio.Models.Project.FileSystem;

namespace RedHerring.Studio.Models.Project.Importers;

// single imported file parsed by importers that will produce output data items and may also read them and process them further
public sealed class ImporterData
{
	public readonly ProjectFileNode InputFileNode;

	public List<ImporterDataItem> OutputDataItems = new();

	public ImporterData(ProjectFileNode inputFileNode)
	{
		InputFileNode = inputFileNode;
	}
}