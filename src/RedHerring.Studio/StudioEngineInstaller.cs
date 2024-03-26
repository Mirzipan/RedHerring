using RedHerring.Core;
using RedHerring.Infusion;
using Silk.NET.Windowing;

namespace RedHerring.Studio;

public class StudioEngineInstaller : BindingsInstaller
{
	private readonly IWindow? _window;

	public StudioEngineInstaller(IWindow? window)
	{
		_window = window;
	}

	public void InstallBindings(ContainerDescription description)
	{
		description.AddCoreSystems();
		description.AddDefinitions();
		description.AddInput(_window);
		description.AddGraphics();
		description.AddStudio();
	}
}
