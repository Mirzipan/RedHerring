using RedHerring.Core;
using RedHerring.ImGui;
using RedHerring.Infusion;

namespace RedHerring.Examples;

public class ExampleEngineInstaller : IBindingsInstaller
{
    public void InstallBindings(ContainerDescription description)
    {
        description.AddGraphics().AddInput().AddSystem(new ImGuiSystem());
    }
}