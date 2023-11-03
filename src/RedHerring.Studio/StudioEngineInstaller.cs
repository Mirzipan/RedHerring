using RedHerring.Core;
using RedHerring.Infusion;

namespace RedHerring.Studio;

public class StudioEngineInstaller : IBindingsInstaller
{
	public void InstallBindings(ContainerDescription description)
	{
		description.AddGraphics().AddInput().AddImGui();
		description.AddStudio();
	}
}
