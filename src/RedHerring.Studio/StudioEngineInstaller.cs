using RedHerring.Core;
using RedHerring.ImGui;
using RedHerring.Infusion;
using RedHerring.Studio.Systems;

namespace RedHerring.Studio;

public class StudioEngineInstaller : IBindingsInstaller
{
	public void InstallBindings(ContainerDescription description)
	{
		description.AddGraphics().AddInput().AddSystem(new ImGuiSystem());
		description.AddSystem(new EditorSystem());
	}
}