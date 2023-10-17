using RedHerring.Engines;
using RedHerring.Engines.Components;

namespace RedHerring.Examples;

public class ExampleEngineContext : AnEngineContext
{
    public ExampleEngineContext()
    {
        Components.Add(new ComponentReference(typeof(GraphicsComponent)));
        Components.Add(new ComponentReference(typeof(InputComponent)));

        UseSeparateRenderThread = true;
    }
}