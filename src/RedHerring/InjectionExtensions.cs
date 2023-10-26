using RedHerring.Alexandria;
using RedHerring.Core;
using RedHerring.Game;
using RedHerring.Infusion;
// ReSharper disable SuspiciousTypeConversion.Global

namespace RedHerring;

public static class InjectionExtensions
{
    public static ContainerDescription AddSystem(this ContainerDescription @this, AnEngineSystem system)
    {
        var types = new List<Type> { system.GetType(), typeof(AnEngineSystem) };

        if (system is IUpdatable)
        {
            types.Add(typeof(IUpdatable));
        }

        if (system is IDrawable)
        {
            types.Add(typeof(IDrawable));
        }
        
        @this.AddInstance(system, types.ToArray());

        return @this;
    }
    
    public static ContainerDescription AddSessionComponent(this ContainerDescription @this, ASessionComponent component)
    {
        var types = new List<Type> { component.GetType(), typeof(ASessionComponent) };

        if (component is IUpdatable)
        {
            types.Add(typeof(IUpdatable));
        }

        if (component is IDrawable)
        {
            types.Add(typeof(IDrawable));
        }
        
        @this.AddInstance(component, types.ToArray());

        return @this;
    }
}