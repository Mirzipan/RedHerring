namespace RedHerring.Core.Systems;

public sealed class PathsSystem : EngineSystem
{
	private static readonly string? _homePath =
		(Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
			? (Environment.GetEnvironmentVariable("XDG_CONFIG_HOME") ?? "~/Library/Application Support")
			: Environment.ExpandEnvironmentVariables("%APPDATA%");
	
	public string ApplicationDataPath { get; private set; } = null!;
	
	public string ResourcesPath { get; private set; } = "Resources";
	
	protected override void Init()
	{
		ApplicationDataPath = Path.Join(_homePath, Context.Name);
	}
}