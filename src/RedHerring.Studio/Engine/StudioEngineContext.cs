using RedHerring.Core;
using RedHerring.Core.Components;
using RedHerring.Studio.Engine.Components;

namespace RedHerring.Studio.Engine;

public class StudioEngineContext : AnEngineContext
{
    public StudioEngineContext()
    {
        Components.Add(new ComponentReference(typeof(GraphicsComponent)));
        Components.Add(new ComponentReference(typeof(InputComponent)));
        Components.Add(new ComponentReference(typeof(EditorComponent)));

        UseSeparateRenderThread = true;
    }
}