using RedHerring.Core;
using RedHerring.Core.Systems;
using RedHerring.ImGui;

namespace RedHerring.Examples;

public class ExampleEngineContext : AnEngineContext
{
    public ExampleEngineContext()
    {
        Systems.Add(new SystemReference(typeof(GraphicsSystem)));
        Systems.Add(new SystemReference(typeof(InputSystem)));
        Systems.Add(new SystemReference(typeof(ImGuiSystem)));

        UseSeparateRenderThread = true;
    }
}