namespace RedHerring.Studio.Models.Project.Importers;

[Importer(".png")]
public sealed class PngImporter : AssetImporter<PngIntermediate, SceneImporterSettings>
{
    protected override PngIntermediate Import(Stream stream, SceneImporterSettings settings)
    {
        using var image = Image.Load(stream);
        return new PngIntermediate(image);
    }
}