using Migration;
using RedHerring.Render;

namespace RedHerring.Studio;

[Serializable, SerializedClassId("663d4118-54fd-4b07-9664-93a4938a1fc3")]
public sealed class ImageImporterSettings : ImporterSettings
{
	public ImagePixelFormat Format          = ImagePixelFormat.R8_G8_B8_A8;
	public bool             GenerateMipMaps = false;
}

#region Migration
[MigratableInterface(typeof(ImageImporterSettings))]
public interface IImageImporterSettingsMigratable : IImporterSettingsMigratable;

[Serializable, LatestVersion(typeof(ImageImporterSettings))]
public class ImageImporterSettings_000 : ImporterSettings_000, IImageImporterSettingsMigratable
{
	public ImagePixelFormat Format;
	public bool             GenerateMipMaps;
}
#endregion