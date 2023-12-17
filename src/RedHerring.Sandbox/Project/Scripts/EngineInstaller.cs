using RedHerring.Core;
using RedHerring.Infusion;

namespace RedHerring.Sandbox;

public class EngineInstaller : BindingsInstaller
{
    public void InstallBindings(ContainerDescription description)
    {
        description.AddCoreSystems().AddGraphics().AddInput();
        description.AddSystem<Configuration>();
    }
}