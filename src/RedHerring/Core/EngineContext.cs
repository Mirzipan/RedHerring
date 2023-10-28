using System.Reflection;
using RedHerring.Alexandria;
using RedHerring.Alexandria.Disposables;
using RedHerring.Deduction;
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
    
    private readonly List<IUpdatable> _currentlyUpdatingSystems = new();
    private readonly List<IDrawable> _currentlyDrawingSystems = new();

    private readonly AssemblyCollection _indexedAssemblies = new();
    private readonly List<IBindingsInstaller> _installers = new();

    private Engine? _engine;
    private InjectionContainer _container = null!;

    public Engine? Engine => _engine;
    public InjectionContainer Container => _container;
    public IView View { get; set; } = null!;
    public GraphicsBackend GraphicsBackend { get; set; }
    public bool UseSeparateRenderThread { get; set; }

    #region Lifecycle

    internal void Init(Engine engine)
    {
        _engine = engine;
        
        var description = new ContainerDescription("Engine");
        description.AddInstance(this);
        description.AddMetadata(_indexedAssemblies);
        
        foreach (var installer in _installers)
        {
            installer.InstallBindings(description);
        }

        _container = description.Build();
        
        _updatables.AddRange(_container.ResolveAll<IUpdatable>());
        _drawables.AddRange(_container.ResolveAll<IDrawable>());
        _systems.AddRange(_container.ResolveAll<AnEngineSystem>());
        
        Sort();
        RaiseInitOnSystems();
    }

    internal void Load()
    {
        int count = _systems.Count;
        for (int i = 0; i < count; i++)
        {
            var system = _systems[i];
            system.RaiseLoad();
        }
    }

    internal void Unload()
    {
        int count = _systems.Count;
        for (int i = 0; i < count; i++)
        {
            var system = _systems[i];
            system.RaiseUnload();
        }
    }
    
    internal void Update(GameTime gameTime)
    {
        _currentlyUpdatingSystems.AddRange(_updatables);
        
        int count = _currentlyUpdatingSystems.Count;
        for (int i = 0; i < count; i++)
        {
            var system = _currentlyUpdatingSystems[i];
            if (!system.IsEnabled)
            {
                continue;
            }

            system.Update(gameTime);
        }
        
        _currentlyUpdatingSystems.Clear();
    }

    internal void Draw(GameTime gameTime)
    {
        _currentlyDrawingSystems.AddRange(_drawables);
        
        int count = _currentlyDrawingSystems.Count;
        for (int i = 0; i < count; i++)
        {
            var system = _currentlyDrawingSystems[i];
            if (!system.IsVisible)
            {
                continue;
            }
            
            if (system.BeginDraw())
            {
                system.Draw(gameTime);
                system.EndDraw();
            }
        }
        
        _currentlyDrawingSystems.Clear();
    }

    #endregion Lifecycle

    #region Manipulation

    public void AddAssemblies(IEnumerable<Assembly> assemblies) => _indexedAssemblies.Add(assemblies);

    public void AddAssemblies(params Assembly[] assemblies) => _indexedAssemblies.Add(assemblies);

    public void AddAssembly(Assembly assembly) => _indexedAssemblies.Add(assembly);

    public void AddInstallers(IEnumerable<IBindingsInstaller> installers) => _installers.AddRange(installers);

    public void AddInstallers(params IBindingsInstaller[] installers) => _installers.AddRange(installers);

    public void AddInstaller(IBindingsInstaller installer) => _installers.Add(installer);

    #endregion Manipulation
    
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
            system.SetContext(this);
            AttributeInjector.Inject(system, _container);
            system.RaiseInit();
            system.DisposeWith(this);
        }
    }

    #endregion Private
}