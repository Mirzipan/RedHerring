namespace RedHerring.Studio.Models.Project.Importers;

[Importer(".png")]
public sealed class PngImporter : AssetImporter<PngIntermediate, PngImporterSettings>
{
    protected override PngIntermediate Import(Stream stream, PngImporterSettings settings)
    {
        using var image = Image.Load(stream);
        return new PngIntermediate(image);
    }
}