using RedHerring.Core;
using RedHerring.Infusion;
using Silk.NET.Windowing;

namespace RedHerring.Sandbox;

public class EngineInstaller : BindingsInstaller
{
    private readonly IWindow? _window;

    public EngineInstaller(IWindow? window)
    {
        _window = window;
    }

    public void InstallBindings(ContainerDescription description)
    {
        description.AddCoreSystems().AddGraphics().AddInput(_window);
        description.AddSystem<Configuration>();
    }
}