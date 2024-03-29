﻿using RedHerring.Clues;
using RedHerring.Infusion;
using RedHerring.Studio.Definition;
using RedHerring.Studio.Systems;
using Configuration = RedHerring.Studio.Systems.Configuration;

namespace RedHerring.Studio;

public static class InjectionExtensions
{
    public static ContainerDescription AddDefinitions(this ContainerDescription @this)
    {
        var context = Definitions.CreateContext();
        ThemeDefinition.AddToContext(context);
        @this.AddSingleton<DefinitionsContext>(context);
        return @this;
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