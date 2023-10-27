using RedHerring.Game.Components;
using RedHerring.Infusion;

namespace RedHerring.Examples;

public class ExampleSessionInstaller : IBindingsInstaller
{
    public void InstallBindings(ContainerDescription description)
    {
        description.AddInstance(new WorldComponent());
    }
}