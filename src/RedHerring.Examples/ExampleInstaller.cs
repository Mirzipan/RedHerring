using RedHerring.Core;
using RedHerring.Core.Systems;
using RedHerring.ImGui;
using RedHerring.Infusion;

namespace RedHerring.Examples;

public class ExampleInstaller : IBindingsInstaller
{
    public void InstallBindings(ContainerDescription description)
    {
        description.AddSystem(new GraphicsSystem());
        description.AddSystem(new InputSystem());
        description.AddSystem(new ImGuiSystem());
    }
}