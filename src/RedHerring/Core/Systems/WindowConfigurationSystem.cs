using System.Text.Json;
using RedHerring.Infusion.Attributes;
using Silk.NET.Maths;
using Silk.NET.Windowing;

namespace RedHerring.Core.Systems;

public sealed class WindowConfigurationSystem : EngineSystem
{
	private const string _configurationFileName = "window_settings.json";
	
	[Serializable]
	public sealed class Data
	{
		public string WindowState     = "Normal";
		public string WindowBorder    = "Resizable";
		public int    WindowPositionX = 100;
		public int    WindowPositionY = 100;
		public int    WindowSizeX     = 640;
		public int    WindowSizeY     = 480;
	}
	
	[Infuse] private PathsSystem _paths = null!;
	
	private string _configFilePath = null!;

	protected override void Init()
	{
		_configFilePath = Path.Join(_paths.ApplicationData, _configurationFileName);
	}
	
	protected override ValueTask<int> Load()
	{
		if(!File.Exists(_configFilePath))
		{
			return ValueTask.FromResult(-1);
		}

		Data? data;
		try
		{
			string json = File.ReadAllText(_configFilePath);

			JsonSerializerOptions options = new()
			                                {
				                                IncludeFields = true
			                                };
			
			data = JsonSerializer.Deserialize<Data>(json, options);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return ValueTask.FromResult(-1);
		}

		if (data == null)
		{
			return ValueTask.FromResult(-1);
		}

		Context.Window.Position     = new Vector2D<int>(data.WindowPositionX, data.WindowPositionY);
		Context.Window.Size         = new Vector2D<int>(data.WindowSizeX,     data.WindowSizeY);
		Context.Window.WindowState  = Enum.TryParse(data.WindowState,  out WindowState windowState) ? windowState : WindowState.Normal;
		Context.Window.WindowBorder = Enum.TryParse(data.WindowBorder, out WindowBorder windowBorder) ? windowBorder : WindowBorder.Resizable;
		
		return ValueTask.FromResult(0);
	}

	protected override ValueTask<int> Unload()
	{
		if (!Directory.Exists(_paths.ApplicationData))
		{
			Directory.CreateDirectory(_paths.ApplicationData);
		}

		Data data = new()
		                  {
			                  WindowState     = Context.Window.WindowState.ToString(),
			                  WindowBorder    = Context.Window.WindowBorder.ToString(),
			                  WindowPositionX = Context.Window.Position.X,
			                  WindowPositionY = Context.Window.Position.Y,
			                  WindowSizeX     = Context.Window.Size.X,
			                  WindowSizeY     = Context.Window.Size.Y,
		                  };

		try
		{
			JsonSerializerOptions options = new()
			                                {
				                                IncludeFields = true
			                                };
			
			string json = JsonSerializer.Serialize(data, options);
			File.WriteAllText(_configFilePath, json);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return ValueTask.FromResult(-1);
		}

		return ValueTask.FromResult(0);
	}
}