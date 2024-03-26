using System.Runtime.InteropServices;
using RedHerring.Alexandria.Extensions.Collections;

namespace RedHerring.Core.Systems;

public sealed class PathsSystem : EngineSystem
{
	public const string ResourcesFolderName = "Resources";
	
	private static string _homePath = null!;
	
	public string ApplicationData { get; private set; } = null!;
	
	public string Resources { get; private set; } = ResourcesFolderName;
	
	protected override void Init()
	{
		InitHomePath();
		ApplicationData = Path.Join(_homePath, Context.Name);
	}

	private void InitHomePath()
	{
		string? path = null;
		
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			path = Environment.ExpandEnvironmentVariables("%APPDATA%");
		}

		if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
		{
			path = Environment.ExpandEnvironmentVariables("XDG_CONFIG_HOME");

			if (path.IsNullOrEmpty())
			{
				path = Environment.ExpandEnvironmentVariables("~/Library/Application Support");
			}
		}
		
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		{
			path = Environment.ExpandEnvironmentVariables("XDG_CONFIG_HOME");
		}
		
		if (path.IsNullOrEmpty())
		{
			path = "./";
		}

		_homePath = path!;
	}

	public static string ShaderCompilerPath()
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			return "Tools\\win-x64\\glslc.exe";
		}

		if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
		{
			return "Tools/osx/glslc";
		}
		
		// if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		// {
		// }

		throw new NotSupportedException();
	}
}