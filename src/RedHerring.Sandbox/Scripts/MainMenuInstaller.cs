using RedHerring.Infusion;
using RedHerring.Sandbox.Menus;

namespace RedHerring.Sandbox;

public class MainMenuInstaller : BindingsInstaller
{
    public void InstallBindings(ContainerDescription description)
    {
        description.AddSessionComponent<MainMenuComponent>();
    }
}