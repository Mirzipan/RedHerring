using RedHerring.Alexandria;
using RedHerring.Alexandria.Disposables;
using RedHerring.Infusion;
using RedHerring.Infusion.Injectors;
using Silk.NET.Windowing;
using Veldrid;

namespace RedHerring.Core;

/// <summary>
/// Context used for running the engine.
/// </summary>
public sealed class EngineContext : AThingamabob
{
    private readonly List<AnEngineSystem> _systems = new();
    private readonly List<IUpdatable> _updatables = new();
    private readonly List<IDrawable> _drawables = new();
    
    private readonly List<IUpdatable> _currentlyUpdatingComponents = new();
    private readonly List<IDrawable> _currentlyDrawingComponents = new();

    private Engine? _engine;
    private InjectionContainer _container = null!;

    public Engine? Engine => _engine;
    public InjectionContainer Container => _container;
    public IView View { get; set; } = null!;
    public GraphicsBackend GraphicsBackend { get; set; }
    public bool UseSeparateRenderThread { get; set; }

    #region Lifecycle

    public void InstallBindings(List<IBindingsInstaller> installers)
    {
        var description = new ContainerDescription("Engine");
        foreach (var installer in installers)
        {
            installer.InstallBindings(description);
        }

        _container = description.Build();
        
        _updatables.AddRange(_container.ResolveAll<IUpdatable>());
        _drawables.AddRange(_container.ResolveAll<IDrawable>());
        _systems.AddRange(_container.ResolveAll<AnEngineSystem>());
    }

    public void Init(Engine engine)
    {
        _engine = engine;

        Sort();
        RaiseInitOnSystems();
    }

    internal void Load()
    {
        int count = _systems.Count;
        for (int i = 0; i < count; i++)
        {
            var component = _systems[i];
            component.RaiseLoad();
        }
    }

    internal void Unload()
    {
        int count = _systems.Count;
        for (int i = 0; i < count; i++)
        {
            var component = _systems[i];
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
    
    #region Private

    private void Sort()
    {
        _updatables.Sort(Comparison.Updatables);
        _drawables.Sort(Comparison.Drawables);
    }

    private void RaiseInitOnSystems()
    {
        if (_systems.Count == 0)
        {
            return;
        }
        
        int count = _systems.Count;
        for (int i = 0; i < count; i++)
        {
            var system = _systems[i];
            system.SetContainer(this);
            AttributeInjector.Inject(system, _container);
            system.RaiseInit();
            system.DisposeWith(this);
        }
    }

    #endregion Private
}