using System.Runtime.CompilerServices;

namespace RedHerring.Motive.Entities;

public static class EntityExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T? Get<T>(this Entity @this) where T : AnEntityComponent
    {
        return @this.Components.Get<T>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGet<T>(this Entity @this, out T? component) where T : AnEntityComponent
    {
        return @this.Components.TryGet(out component);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGet(this Entity @this, Type type, out AnEntityComponent? component)
    {
        return @this.Components.TryGet(type, out component);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TComponent GetOrCreate<TComponent>(this Entity @this) where TComponent : AnEntityComponent, new()
    {
        return @this.Components.GetOrCreate<TComponent>();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<TComponent> GetAll<TComponent>(this Entity @this) where TComponent : AnEntityComponent 
    {
        return @this.Components.GetAll<TComponent>();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Has<TComponent>(this Entity @this) where TComponent : AnEntityComponent 
    {
       return @this.Components.Has<TComponent>();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Has(this Entity @this, Type type) 
    {
        return @this.Components.Has(type);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf<TComponent>(this Entity @this) where TComponent : AnEntityComponent 
    {
        return @this.Components.IndexOf<TComponent>();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf(this Entity @this, Type type) 
    {
        return @this.Components.IndexOf(type);
    }
    
}