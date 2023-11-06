using RedHerring.Core;
using RedHerring.ImGui;
using RedHerring.Infusion;

namespace RedHerring.Sandbox;

public class SandboxEngineInstaller : BindingsInstaller
{
    public void InstallBindings(ContainerDescription description)
    {
        description.AddGraphics().AddInput().AddSystem(new ImGuiSystem());
    }
}