namespace RedHerring.Studio.Models.Project.Importers;

// fallback importer if no other importer is found
[Importer]
public sealed class CopyImporter : AssetImporter<CopyIntermediate, CopyImporterSettings>
{
	protected override CopyIntermediate Import(Stream stream, CopyImporterSettings settings)
	{
		byte[] result = new byte[stream.Length];
		int read   = stream.Read(result, 0, result.Length);
		// TODO report error if read != result.Length?
		
		return new CopyIntermediate(result);
	}
}