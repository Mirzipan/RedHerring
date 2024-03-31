using RedHerring;
using RedHerring.Core;
using RedHerring.Infusion;
using Silk.NET.Windowing;

namespace GameLibrary;

public class GameInstaller : BindingsInstaller
{
	private readonly IWindow? _window;
	
	public GameInstaller(IWindow? window)
	{
		_window = window;
	}
	
	public void InstallBindings(ContainerDescription description)
	{
		description.AddCoreSystems();
		description.AddDefinitions();
		description.AddInput(_window);
		description.AddGraphics();
	}
}