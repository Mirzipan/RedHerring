using System.Collections;
using RedHerring.Alexandria;
using RedHerring.Alexandria.Components;
using RedHerring.Alexandria.Extensions.Collections;
using RedHerring.Exceptions;
using RedHerring.Infusion;
using RedHerring.Infusion.Injectors;

namespace RedHerring.Game;

public sealed class SessionComponentCollection : ISessionComponentCollection, IDisposable
{
    public Session Session { get; }
    
    private readonly Dictionary<Type, ASessionComponent> _componentIndex = new();
    private readonly List<ASessionComponent> _components = new();
    
    private readonly List<IUpdatable> _updatables = new();
    private readonly List<IDrawable> _drawables = new();
    private readonly List<IBindingsInstaller> _installers = new();
    
    private readonly List<IUpdatable> _currentlyUpdatingComponents = new();
    private readonly List<IDrawable> _currentlyDrawingComponents = new();

    #region Lifecycle
    
    public SessionComponentCollection(Session session)
    {
        Session = session;
    }

    internal void Init()
    {
        if (Session.Context!.Components.IsNullOrEmpty())
        {
            return;
        }

        int count = Session.Context.Components.Count;
        for (int i = 0; i < count; i++)
        {
            var componentType = Session.Context.Components[i];
            if (!componentType.IsSubclassOf(typeof(ASessionComponent)))
            {
                throw new TypeIsNotAGameComponentException(componentType);
            }
            
            object? instance = Activator.CreateInstance(componentType);
            if (instance is null)
            {
                throw new NullReferenceException();
            }

            var componentInstance = (instance as ASessionComponent)!;
            _components.Add(componentInstance);
            _componentIndex[componentType] = componentInstance;

            TryAddSpecializedComponent(componentInstance);
        }
        
        Sort();
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
            AttributeInjector.Inject(component, Session.InjectionContainer);
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

    public void Dispose()
    {
        int count = _components.Count;
        _components.Clear();
    }

    #endregion Lifecycle

    #region Manipulation

    private void Sort()
    {
        _updatables.Sort(Comparison.Updatables);
        _drawables.Sort(Comparison.Drawables);
    }

    #endregion Manipulation

    #region Queries
    
    IComponent? IComponentContainer.Get(Type type)
    {
        return _componentIndex.TryGetValue(type, out var value) ? value : null;
    }

    public T? Get<T>() where T : ASessionComponent
    {
        return _componentIndex.TryGetValue(typeof(T), out var value) ? (T)value : null;
    }

    public bool TryGet<T>(out T? component) where T : ASessionComponent
    {
        if (_componentIndex.TryGetValue(typeof(T), out var value))
        {
            component = (T)value;
            return true;
        }

        component = null;
        return false;
    }

    public bool TryGet(Type type, out ASessionComponent? component)
    {
        return _componentIndex.TryGetValue(type, out component);
    }
    
    #endregion Queries

    #region Private

    private void TryAddSpecializedComponent(ASessionComponent component)
    {
        if (component is IUpdatable updatable)
        {
            _updatables.Add(updatable);
        }

        if (component is IDrawable drawable)
        {
            _drawables.Add(drawable);
        }

        if (component is IBindingsInstaller installer)
        {
            _installers.Add(installer);
        }
    }

    #endregion Private

    #region IEnumerable

    public IEnumerator<ASessionComponent> GetEnumerator()
    {
        return _components.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _components.GetEnumerator();
    }
    
    #endregion IEnumerable
}