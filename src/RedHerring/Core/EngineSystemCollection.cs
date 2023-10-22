using System.Collections;
using RedHerring.Alexandria;
using RedHerring.Alexandria.Components;
using RedHerring.Alexandria.Extensions.Collections;
using RedHerring.Exceptions;
using RedHerring.Infusion;
using RedHerring.Infusion.Injectors;

namespace RedHerring.Core;

public sealed class EngineSystemCollection : IEngineComponentCollection
{
    private readonly Dictionary<Type, AnEngineSystem> _componentIndex = new();
    private readonly List<AnEngineSystem> _components = new();
    private readonly Engine _engine;
    
    private readonly List<IUpdatable> _updatables = new();
    private readonly List<IDrawable> _drawables = new();
    private readonly List<IBindingsInstaller> _installers = new();
    
    private readonly List<IUpdatable> _currentlyUpdatingComponents = new();
    private readonly List<IDrawable> _currentlyDrawingComponents = new();

    public Engine Engine => _engine;

    #region Lifecycle

    public EngineSystemCollection(Engine engine)
    {
        _engine = engine;
    }

    internal void Init()
    {
        if (_engine.Context.Systems.IsNullOrEmpty())
        {
            return;
        }

        int count = _engine.Context.Systems.Count;
        for (int i = 0; i < count; i++)
        {
            var component = _engine.Context.Systems[i];
            if (!component.Type.IsSubclassOf(typeof(AnEngineSystem)))
            {
                throw new TypeIsNotAnEngineComponentException(component.Type);
            }
            
            object? instance = Activator.CreateInstance(component.Type);
            if (instance is null)
            {
                throw new NullReferenceException();
            }

            var componentInstance = (instance as AnEngineSystem)!;
            _components.Add(componentInstance);
            _componentIndex[component.Type] = componentInstance;

            TryAddSpecializedComponent(componentInstance);
        }
        
        Sort();
        RaiseInitOnComponents();
    }

    internal void InstallBindings(ContainerDescription description)
    {
        for (int i = 0; i < _components.Count; i++)
        {
            var component = _components[i];
            description.AddInstance(component);
        }
        
        if (_installers.Count == 0)
        {
            return;
        }

        for (int i = 0; i < _installers.Count; i++)
        {
            var installer = _installers[i];
            installer.InstallBindings(description);
        }
    }

    internal void Load()
    {
        int count = _components.Count;
        for (int i = 0; i < count; i++)
        {
            var component = _components[i];
            AttributeInjector.Inject(component, Engine.InjectionContainer);
        }
        
        for (int i = 0; i < count; i++)
        {
            var component = _components[i];
            component.RaiseLoad();
        }
    }

    internal void Unload()
    {
        int count = _components.Count;
        for (int i = 0; i < count; i++)
        {
            var component = _components[i];
            component.RaiseUnload();
        }
    }
    
    internal void Update(GameTime gameTime)
    {
        _currentlyUpdatingComponents.AddRange(_updatables);
        
        int count = _currentlyUpdatingComponents.Count;
        for (int i = 0; i < count; i++)
        {
            var component = _currentlyUpdatingComponents[i];
            if (!component.IsEnabled)
            {
                continue;
            }

            component.Update(gameTime);
        }
        
        _currentlyUpdatingComponents.Clear();
    }

    internal void Draw(GameTime gameTime)
    {
        _currentlyDrawingComponents.AddRange(_drawables);
        
        int count = _currentlyDrawingComponents.Count;
        for (int i = 0; i < count; i++)
        {
            var component = _currentlyDrawingComponents[i];
            if (!component.IsVisible)
            {
                continue;
            }
            
            if (component.BeginDraw())
            {
                component.Draw(gameTime);
                component.EndDraw();
            }
        }
        
        _currentlyDrawingComponents.Clear();
    }

    #endregion Lifecycle

    #region Queries

    IComponent? IComponentContainer.Get(Type type)
    {
        return _componentIndex.TryGetValue(type, out var value) ? value : null;
    }

    public TEngineComponent? Get<TEngineComponent>() where TEngineComponent : AnEngineSystem
    {
        return _componentIndex.TryGetValue(typeof(TEngineComponent), out var value) ? (TEngineComponent)value : null;
    }

    public bool TryGet<TEngineComponent>(out TEngineComponent? component) where TEngineComponent : AnEngineSystem
    {
        if (_componentIndex.TryGetValue(typeof(TEngineComponent), out var value))
        {
            component = (TEngineComponent)value;
            return true;
        }

        component = null;
        return false;
    }

    public bool TryGet(Type type, out AnEngineSystem? component)
    {
        return _componentIndex.TryGetValue(type, out component);
    }
    
    #endregion Queries
    
    #region Manipulation

    private void Sort()
    {
        _updatables.Sort(Comparison.Updatables);
        _drawables.Sort(Comparison.Drawables);
    }

    #endregion Manipulation

    #region Private

    private void TryAddSpecializedComponent(AnEngineSystem system)
    {
        if (system is IUpdatable updatable)
        {
            _updatables.Add(updatable);
        }

        if (system is IDrawable drawable)
        {
            _drawables.Add(drawable);
        }

        if (system is IBindingsInstaller installer)
        {
            _installers.Add(installer);
        }
    }

    private void RaiseInitOnComponents()
    {
        int count = _components.Count;
        for (int i = 0; i < count; i++)
        {
            var component = _components[i];
            component.SetContainer(this);
            component.RaiseInit();
        }
    }

    #endregion Private

    #region IEnumerable

    IEnumerator<AnEngineSystem> IEnumerable<AnEngineSystem>.GetEnumerator()
    {
        return _components.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _components.GetEnumerator();
    }
    
    #endregion IEnumerable
}