using RedHerring.Game.Components;
using RedHerring.Infusion;
using RedHerring.Sandbox.Game.Session;

namespace RedHerring.Sandbox.Game;

public class GameSessionInstaller : BindingsInstaller
{
    public void InstallBindings(ContainerDescription description)
    {
        description.AddSingleton<GameMenuComponent>();
        description.AddInstance(new WorldComponent());
    }
}