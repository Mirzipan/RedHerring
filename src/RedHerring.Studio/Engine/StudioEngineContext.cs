using RedHerring.Core;
using RedHerring.Core.Systems;
using RedHerring.Studio.Engine.Systems;

namespace RedHerring.Studio.Engine;

public class StudioEngineContext : AnEngineContext
{
    public StudioEngineContext()
    {
        Systems.Add(new SystemReference(typeof(GraphicsSystem)));
        Systems.Add(new SystemReference(typeof(InputSystem)));
        Systems.Add(new SystemReference(typeof(EditorSystem)));

        UseSeparateRenderThread = true;
    }
}