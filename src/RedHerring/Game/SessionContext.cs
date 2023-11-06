using RedHerring.Alexandria;
using RedHerring.Alexandria.Disposables;
using RedHerring.Core;
using RedHerring.Infusion;
using RedHerring.Infusion.Injectors;

namespace RedHerring.Game;

public sealed class SessionContext : ANamedDisposer
{
    private readonly List<ASessionComponent> _components = new();
    private readonly List<IUpdatable> _updatables = new();
    private readonly List<IDrawable> _drawables = new();
    
    private readonly List<IUpdatable> _currentlyUpdatingComponents = new();
    private readonly List<IDrawable> _currentlyDrawingComponents = new();

    private readonly List<BindingsInstaller> _installers = new();

    private Engine? _engine;
    private Session? _session;
    private InjectionContainer _container = null!;

    public Engine? Engine => _engine;
    public Session? Session => _session;
    public InjectionContainer Container => _container;

    #region Lifecycle

    internal void Init(Engine engine, Session session)
    {
        _engine = engine;
        _session = session;

        InstallBindings();
        ResolveComponents();
        
        Sort();
        RaiseInitOnComponents();
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

    #region Manipulation

    public SessionContext WithInstallers(IEnumerable<BindingsInstaller> installers)
    {
        _installers.AddRange(installers);
        return this;
    }

    public SessionContext WithInstallers(params BindingsInstaller[] installers)
    {
        _installers.AddRange(installers);
        return this;
    }

    public SessionContext WithInstaller(BindingsInstaller installer)
    {
        _installers.Add(installer);
        return this;
    }

    #endregion Manipulation
    
    #region Private

    private void InstallBindings()
    {
        var description = new ContainerDescription("Session", _engine!.Context.Container);
        description.AddInstance(this);
        description.AddInstance(_engine);
        
        foreach (var installer in _installers)
        {
            installer.InstallBindings(description);
        }

        _container = description.Build();
    }

    private void ResolveComponents()
    {
        _components.AddRange(_container.ResolveAll<ASessionComponent>());
        int count = _components.Count;
        if (count == 0)
        {
            return;
        }
        
        for (int i = 0; i < count; i++)
        {
            var component = _components[i];
            if (component is IUpdatable updatable)
            {
                _updatables.Add(updatable);
            }
            
            if (component is IDrawable drawable)
            {
                _drawables.Add(drawable);
            }
        }
    }

    private void Sort()
    {
        _updatables.Sort(Comparison.Updatables);
        _drawables.Sort(Comparison.Drawables);
    }

    private void RaiseInitOnComponents()
    {
        if (_components.Count == 0)
        {
            return;
        }
        
        int count = _components.Count;
        for (int i = 0; i < count; i++)
        {
            var component = _components[i];
            component.SetContext(this);
            AttributeInjector.Inject(component, _container);
            component.DisposeWith(this);
        }
    }

    #endregion Private
}