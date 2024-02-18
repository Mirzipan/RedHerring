using RedHerring.Core;
using RedHerring.Infusion;

namespace RedHerring.Studio;

public class StudioEngineInstaller : BindingsInstaller
{
	public void InstallBindings(ContainerDescription description)
	{
		description.AddCoreSystems();
		description.AddDefinitions();
		description.AddGraphics();
		description.AddInput();
		description.AddStudio();
	}
}
