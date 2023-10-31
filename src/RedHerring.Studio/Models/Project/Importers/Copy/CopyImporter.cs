namespace RedHerring.Studio.Models.Project.Importers;

// fallback importer if no other importer is found
[Importer]
public sealed class CopyImporter : AnImporter<CopyIntermediate>
{
	public override CopyIntermediate Import(Stream stream)
	{
		return new CopyIntermediate();
	}
}