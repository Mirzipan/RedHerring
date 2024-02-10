using RedHerring.Clues;
using RedHerring.Core;
using RedHerring.Infusion;
using RedHerring.Studio.Definition;

namespace RedHerring.Studio;

public class StudioEngineInstaller : BindingsInstaller
{
	public void InstallBindings(ContainerDescription description)
	{
		var set = new DefinitionSet();
		ThemeDefinition.AddToSet(set);
		Definitions.CreateContext(set);
		description.AddCoreSystems();
		description.AddGraphics();
		description.AddInput();
		description.AddStudio();
	}
}
