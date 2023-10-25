using RedHerring.Alexandria;
using RedHerring.Infusion;

namespace RedHerring.Core;

public static class InjectionExtensions
{
    public static ContainerDescription AddSystem(this ContainerDescription @this, AnEngineSystem system)
    {
        var types = new List<Type> { typeof(AnEngineSystem) };

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
}