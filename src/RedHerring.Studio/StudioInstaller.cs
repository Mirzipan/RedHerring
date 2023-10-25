using RedHerring.Core;
using RedHerring.Core.Systems;
using RedHerring.ImGui;
using RedHerring.Infusion;
using RedHerring.Studio.Systems;

namespace RedHerring.Studio;

public class StudioInstaller : IBindingsInstaller
{
	public void InstallBindings(ContainerDescription description)
	{
		description.AddSystem(new GraphicsSystem());
		description.AddSystem(new InputSystem());
		description.AddSystem(new ImGuiSystem());
		description.AddSystem(new EditorSystem());
	}
}