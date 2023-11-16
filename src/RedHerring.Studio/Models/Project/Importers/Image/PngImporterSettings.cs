using Migration;
using SixLabors.ImageSharp.Formats.Png;

namespace RedHerring.Studio.Models.Project.Importers;

[Serializable, SerializedClassId("png-importer-id")]
public sealed class PngImporterSettings : ImporterSettings
{
    public PngBitDepth BitDepth;
    public PngColorType ColorType;
    public PngCompressionLevel Compression;
    public bool UseGamma;
    public bool PreserveTransparentColors;
}