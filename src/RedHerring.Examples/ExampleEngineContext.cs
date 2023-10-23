using RedHerring.Core;
using RedHerring.Core.Systems;
using RedHerring.ImGui;

namespace RedHerring.Examples;

public class ExampleEngineContext : AnEngineContext
{
    public ExampleEngineContext()
    {
        Systems.Add(SystemReference.Create<GraphicsSystem>());
        Systems.Add(SystemReference.Create<InputSystem>());
        Systems.Add(SystemReference.Create<ImGuiSystem>());

        UseSeparateRenderThread = true;
    }
}