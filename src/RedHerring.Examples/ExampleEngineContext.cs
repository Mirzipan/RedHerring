using RedHerring.Core;
using RedHerring.Core.Components;
using RedHerring.ImGui;

namespace RedHerring.Examples;

public class ExampleEngineContext : AnEngineContext
{
    public ExampleEngineContext()
    {
        Components.Add(new ComponentReference(typeof(GraphicsComponent)));
        Components.Add(new ComponentReference(typeof(InputComponent)));
        Components.Add(new ComponentReference(typeof(ImGuiComponent)));

        UseSeparateRenderThread = true;
    }
}