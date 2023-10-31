using Migration;

namespace RedHerring.Studio.Models;

[Serializable, SerializedClassId("options-class-id")]
public sealed class OptionsModel
{
	[NonSerialized] public static string? HomeDirectory = (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
		? Environment.GetEnvironmentVariable("HOME")
		: Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
	
	[NonSerialized] public static string? OptionsPath = Path.Join(HomeDirectory, "RedHerring", "options.json");
	
	
}

#region Migration
[MigratableInterface(typeof(OptionsModel))]
public interface IOptionsModelMigratable
{
}
    
[Serializable, LatestVersion(typeof(OptionsModel))]
public class OptionsModel_000 : IOptionsModelMigratable
{
}
#endregion