using Migration;

namespace RedHerring.Studio;

[Serializable, SerializedClassId("733fc598-ff87-41c5-8f11-997f25d715a5")]
public sealed class ShaderImporterSettings : ImporterSettings
{
	public string            EntryPoint  = "main";
	public ShaderImporterStage ShaderStage = ShaderImporterStage.vertex;
}

#region Migration
[MigratableInterface(typeof(ShaderImporterSettings))]
public interface IShaderImporterSettingsMigratable : IImporterSettingsMigratable;

[Serializable, LatestVersion(typeof(ShaderImporterSettings))]
public class ShaderImporterSettings_000 : ImporterSettings_000, IShaderImporterSettingsMigratable
{
	public string            EntryPoint;
	public ShaderImporterStage ShaderStage;
}
#endregion