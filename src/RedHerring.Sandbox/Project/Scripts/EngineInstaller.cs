using RedHerring.Core;
using RedHerring.ImGui;
using RedHerring.Infusion;

namespace RedHerring.Sandbox;

public class EngineInstaller : BindingsInstaller
{
    public void InstallBindings(ContainerDescription description)
    {
        description.AddGraphics().AddInput().AddSystem(new ImGuiSystem());
    }
}