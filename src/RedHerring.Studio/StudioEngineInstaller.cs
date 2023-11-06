using RedHerring.Core;
using RedHerring.Infusion;

namespace RedHerring.Studio;

public class StudioEngineInstaller : IBindingsInstaller
{
	public void InstallBindings(ContainerDescription description)
	{
		description.AddGraphics();
		description.AddInput().AddImGui();
		description.AddStudio();
	}
}
