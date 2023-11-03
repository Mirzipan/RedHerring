using RedHerring.ImGui;
using RedHerring.Infusion;
using RedHerring.Studio.Systems;

namespace RedHerring.Studio;

public static class InjectionExtensions
{
    public static ContainerDescription AddImGui(this ContainerDescription @this)
    {
        return @this.AddSystem(new ImGuiSystem());
    }

    public static ContainerDescription AddEditor(this ContainerDescription @this)
    {
        return @this.AddSystem(new StudioSystem());
    }
}