using RedHerring.Game.Components;
using RedHerring.Infusion;

namespace RedHerring.Sandbox;

public class SandboxSessionInstaller : IBindingsInstaller
{
    public void InstallBindings(ContainerDescription description)
    {
        description.AddInstance(new WorldComponent());
    }
}