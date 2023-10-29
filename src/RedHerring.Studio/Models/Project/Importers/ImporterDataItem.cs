namespace RedHerring.Studio.Models.Project.Importers;

// this class represents data that can be exported or processed by another importer
public sealed class ImporterDataItem
{
	public readonly object               Data;
	public readonly string               OutputFilePath;
	public readonly ImporterOutputFormat OutputFormat;
	public          bool                 ShouldBeExported;

	public ImporterDataItem(object data, string outputFilePath, ImporterOutputFormat outputFormat)
	{
		Data             = data;
		OutputFilePath   = outputFilePath;
		OutputFormat     = outputFormat;
		ShouldBeExported = true;
	}
}