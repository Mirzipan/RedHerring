using RedHerring.Infusion;
using RedHerring.Sandbox.MainMenu.Session;

namespace RedHerring.Sandbox.MainMenu;

public class MainSessionInstaller : BindingsInstaller
{
    public void InstallBindings(ContainerDescription description)
    {
        description.AddSessionComponent<MainMenuComponent>();
    }
}