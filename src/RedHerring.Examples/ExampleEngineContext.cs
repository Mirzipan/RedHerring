using RedHerring.Engines;
using RedHerring.Engines.Components;

namespace RedHerring.Examples;

public class ExampleEngineContext : AnEngineContext
{
    public ExampleEngineContext()
    {
        Components = new List<ComponentReference>();
        
        Components.Add(new ComponentReference(typeof(RenderComponent)));
        Components.Add(new ComponentReference(typeof(InputComponent)));
    }
}