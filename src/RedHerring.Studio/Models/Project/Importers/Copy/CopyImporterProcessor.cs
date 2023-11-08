namespace RedHerring.Studio.Models.Project.Importers;

public sealed class CopyImporterProcessor : AssetImporterProcessor<CopyIntermediate, CopyImporterSettings>
{
	protected override void Process(CopyIntermediate? intermediate, CopyImporterSettings settings, string resourcePath)
	{
		if (intermediate == null)
		{
			return;
		}

		Directory.CreateDirectory(Path.GetDirectoryName(resourcePath)!);
		File.WriteAllBytes(resourcePath, intermediate.Data);
	}
}