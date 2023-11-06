using RedHerring.Game.Components;
using RedHerring.Infusion;

namespace RedHerring.Sandbox;

public class SandboxSessionInstaller : BindingsInstaller
{
    public void InstallBindings(ContainerDescription description)
    {
        description.AddInstance(new WorldComponent());
    }
}