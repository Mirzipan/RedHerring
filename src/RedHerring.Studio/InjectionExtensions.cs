using RedHerring.Clues;
using RedHerring.ImGui;
using RedHerring.Infusion;
using RedHerring.Studio.Systems;
using Configuration = RedHerring.Studio.Systems.Configuration;

namespace RedHerring.Studio;

public static class InjectionExtensions
{
    public static ContainerDescription AddImGui(this ContainerDescription @this)
    {
        return @this.AddSystem(new ImGuiSystem());
    }
    
    public static ContainerDescription AddDefinitions(this ContainerDescription @this)
    {
        return @this.AddSystem<DefinitionSystem>();
    }

    public static ContainerDescription AddStudio(this ContainerDescription @this)
    {
        @this.AddSystem<Configuration>();
        
        @this.AddSystem<StudioCamera>();
        @this.AddSystem<StudioGraphics>();
        @this.AddSystem<StudioSystem>();
        
        return @this;
    }
}