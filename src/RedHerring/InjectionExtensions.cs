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

        if (system is Updatable)
        {
            types.Add(typeof(Updatable));
        }

        if (system is Drawable)
        {
            types.Add(typeof(Drawable));
        }
        
        @this.AddInstance(system, types.ToArray());

        return @this;
    }
    
    public static ContainerDescription AddSessionComponent(this ContainerDescription @this, SessionComponent component)
    {
        var types = new List<Type> { component.GetType(), typeof(SessionComponent) };

        if (component is Updatable)
        {
            types.Add(typeof(Updatable));
        }

        if (component is Drawable)
        {
            types.Add(typeof(Drawable));
        }
        
        @this.AddInstance(component, types.ToArray());

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

        if (indexer is IIndexAttributes)
        {
            types.Add(typeof(IIndexAttributes));
        }

        if (indexer is IIndexTypes)
        {
            types.Add(typeof(IIndexTypes));
        }
        
        @this.AddInstance(indexer, types.ToArray());

        return @this;
    }
}