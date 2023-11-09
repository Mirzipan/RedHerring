using RedHerring.Game.Components;
using RedHerring.Infusion;

namespace RedHerring.Sandbox;

public class GameSessionInstaller : BindingsInstaller
{
    public void InstallBindings(ContainerDescription description)
    {
        description.AddInstance(new WorldComponent());
    }
}