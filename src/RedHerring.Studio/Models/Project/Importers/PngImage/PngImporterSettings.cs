/*
using Migration;
using SixLabors.ImageSharp.Formats.Png;

namespace RedHerring.Studio;

[Serializable, SerializedClassId("79e3e75f-a6c2-465b-b815-fd68fb46972a")]
public sealed class PngImporterSettings : ImporterSettings
{
	public override ProjectNodeType NodeType => ProjectNodeType.AssetImage;

    public PngBitDepth BitDepth = PngBitDepth.Bit8;
    public PngColorType ColorType = PngColorType.RgbWithAlpha;
    public PngCompressionLevel Compression = PngCompressionLevel.BestSpeed;
    public bool UseGamma = false;
    public bool PreserveTransparentColors = true;
}

#region Migration
[MigratableInterface(typeof(PngImporterSettings))]
public interface IPngImporterSettingsMigratable : IImporterSettingsMigratable;

[Serializable, LatestVersion(typeof(PngImporterSettings))]
public class PngImporterSettings_000 : ImporterSettings_000, IPngImporterSettingsMigratable
{
    public PngBitDepth BitDepth;
    public PngColorType ColorType;
    public PngCompressionLevel Compression;
    public bool UseGamma;
    public bool PreserveTransparentColors;
}
#endregion
*/