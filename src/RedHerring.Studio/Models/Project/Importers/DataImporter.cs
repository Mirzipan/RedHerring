namespace RedHerring.Studio.Models.Project.Importers;

/// <summary>
/// Default importer that can import any file as a byte array.
/// </summary>
internal sealed class DataImporter : AssetImporter<byte[]>
{
    public override byte[] Import(Stream stream)
    {
        var result = new byte[stream.Length];
        int read = stream.Read(result, 0, result.Length);
        return result;
    }
}