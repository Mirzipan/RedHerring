using RedHerring.Clues;
using RedHerring.Core;
using RedHerring.Infusion;
using RedHerring.Studio.Definition;

namespace RedHerring.Studio;

public class StudioEngineInstaller : BindingsInstaller
{
	public void InstallBindings(ContainerDescription description)
	{
		description.AddDefinitions();
		description.AddCoreSystems();
		description.AddGraphics();
		description.AddInput();
		description.AddStudio();
	}
}
