using RedHerring.Core;
using RedHerring.Core.Systems;
using RedHerring.ImGui;
using RedHerring.Studio.Systems;

namespace RedHerring.Studio;

public class StudioEngineContext : AnEngineContext
{
	public StudioEngineContext()
	{
		Systems.Add(SystemReference.Create<GraphicsSystem>());
		Systems.Add(SystemReference.Create<InputSystem>());
		Systems.Add(SystemReference.Create<ImGuiSystem>());
		Systems.Add(SystemReference.Create<EditorSystem>());

		UseSeparateRenderThread = true;
	}
}