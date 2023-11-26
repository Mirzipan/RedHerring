using RedHerring.Core;
using RedHerring.Infusion;

namespace RedHerring.Studio;

public class StudioEngineInstaller : BindingsInstaller
{
	public void InstallBindings(ContainerDescription description)
	{
		description.AddGraphics();
		description.AddInput().AddImGui();
		description.AddDefinitions();
		description.AddStudio();
	}
}
