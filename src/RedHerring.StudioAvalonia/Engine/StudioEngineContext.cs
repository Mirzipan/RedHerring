using RedHerring.Core;
using RedHerring.Core.Systems;
using RedHerring.Studio.Engine.Systems;

namespace RedHerring.Studio.Engine;

public class StudioEngineContext : AnEngineContext
{
    public StudioEngineContext()
    {
        Systems.Add(SystemReference.Create<GraphicsSystem>());
        Systems.Add(SystemReference.Create<InputSystem>());
        Systems.Add(SystemReference.Create<EditorSystem>());

        UseSeparateRenderThread = true;
    }
}