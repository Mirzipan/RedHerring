namespace RedHerring.Studio.Models.Project.Importers;

public sealed class CopyImporterProcessor : AssetImporterProcessor<CopyIntermediate, CopyImporterSettings>
{
	protected override void Process(CopyIntermediate? intermediate, CopyImporterSettings settings)
	{
		Console.WriteLine("CopyIntermediate");
	}
}