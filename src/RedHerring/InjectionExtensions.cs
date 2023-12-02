using System.Runtime.CompilerServices;
using RedHerring.Alexandria;
using RedHerring.Core;
using RedHerring.Core.Systems;
using RedHerring.Deduction;
using RedHerring.Fingerprint;
using RedHerring.Fingerprint.Layers;
using RedHerring.Game;
using RedHerring.Infusion;
// ReSharper disable SuspiciousTypeConversion.Global

namespace RedHerring;

public static class InjectionExtensions
{
    public static ContainerDescription AddSystem(this ContainerDescription @this, EngineSystem system)
    {
        var types = new List<Type> { system.GetType(), typeof(EngineSystem) };

        AddTypeIfAssignableTo<Updatable>(types, system);
        AddTypeIfAssignableTo<Drawable>(types, system);
        
        @this.AddInstance(system, types.ToArray());

        return @this;
    }    
    
    public static ContainerDescription AddSystem<T>(this ContainerDescription @this) where T : EngineSystem
    {
        var type = typeof(T);
        var types = new List<Type> { type, typeof(EngineSystem) };

        AddTypeIfAssignableTo<Updatable>(types, type);
        AddTypeIfAssignableTo<Drawable>(types, type);
        
        @this.AddSingleton(type, types.ToArray());

        return @this;
    }
    
    public static ContainerDescription AddSessionComponent(this ContainerDescription @this, SessionComponent component)
    {
        var types = new List<Type> { component.GetType(), typeof(SessionComponent) };
        
        AddTypeIfAssignableTo<Updatable>(types, component);
        AddTypeIfAssignableTo<Drawable>(types, component);
        
        @this.AddInstance(component, types.ToArray());

        return @this;
    }    
    
    public static ContainerDescription AddSessionComponent<T>(this ContainerDescription @this) where T : SessionComponent
    {
        var type = typeof(T);
        var types = new List<Type> { type, typeof(SessionComponent) };

        AddTypeIfAssignableTo<Updatable>(types, type);
        AddTypeIfAssignableTo<Drawable>(types, type);
        
        @this.AddSingleton(type, types.ToArray());

        return @this;
    }

    public static ContainerDescription AddInput(this ContainerDescription @this)
    {
        @this.AddSingleton(typeof(SilkInput), typeof(SilkInput), typeof(Input));
        @this.AddSingleton(typeof(InputReceiver));
        @this.AddSystem(new InputSystem());
        return @this;
    }
    
    public static ContainerDescription AddMetadata(this ContainerDescription @this, AssemblyCollection collection)
    {
        var meta = new MetadataDatabase(collection);
        meta.Process();
        @this.AddInstance(meta);

        foreach (var indexer in meta.Indexers)
        {
            AddIndexer(@this, indexer);
        }

        return @this;
    }
    
    public static ContainerDescription AddIndexer(this ContainerDescription @this, IIndexMetadata indexer)
    {
        var types = new List<Type> { indexer.GetType(), typeof(IIndexMetadata) };

        AddTypeIfAssignableTo<IIndexAttributes>(types, indexer);
        AddTypeIfAssignableTo<IIndexTypes>(types, indexer);
        
        @this.AddInstance(indexer, types.ToArray());

        return @this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void AddTypeIfAssignableTo<T>(List<Type> destination, Type type)
    {
        var targetType = typeof(T);
        if (type.IsAssignableTo(targetType))
        {
            destination.Add(targetType);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void AddTypeIfAssignableTo<T>(List<Type> destination, object instance)
    {
        if (instance is T)
        {
            destination.Add(typeof(T));
        }
    }
}