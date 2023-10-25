using RedHerring.Core;
using RedHerring.ImGui;
using RedHerring.Infusion;

namespace RedHerring.Examples;

public class ExampleInstaller : IBindingsInstaller
{
    public void InstallBindings(ContainerDescription description)
    {
        description.AddGraphics().AddInput().AddSystem(new ImGuiSystem());
    }
}