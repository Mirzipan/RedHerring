using RedHerring.Clues;
using RedHerring.Core;
using RedHerring.Game;
using RedHerring.Platforms;
using RedHerring.Render;
using RedHerring.Studio.Systems;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Extensions.Veldrid;
using Veldrid;

namespace RedHerring.Studio;

internal class Program
{
    public const string Title = "Red Herring Engine Studio"; 
    
    private static IWindow? _window;

    private static Engine _engine = null!;
    private static SessionContext _sessionContext = null!;

    private static GraphicsBackend _graphicsBackend;

    private static Platform _platform = null!;

    private static void Main(string[] args)
    {
        _platform = DesktopPlatform.Create("RedHerring Studio");
        
        Init();
        
        var opts = new WindowOptions
        {
            Title = Title,
            Position = new Vector2D<int>(100, 100),
            Size = new Vector2D<int>(960, 540),
            API = _graphicsBackend.ToGraphicsAPI(),
            VSync = true,
            ShouldSwapAutomatically = false,
        };
        
        _window = Window.Create(opts);
        
        _window.Load         += OnLoad;
        _window.Update       += OnUpdate;
        _window.Render       += OnDraw;
        _window.Closing      += OnClose;
        _window.Resize       += OnResize;
        _window.FocusChanged += OnFocusChanged;

        _window.Run();
    }

    #region Private

    private static void Init()
    {
        _graphicsBackend = RenderInstaller.PreferredBackend();
    }

    #endregion Private

    #region Bindings

    static void OnLoad()
    {
        _engine = new Engine();
        _engine.OnExit += OnEngineExit;

        var render = new RenderInstaller(_window!)
        {
            Backend = _graphicsBackend,
            UseSeparateRenderThread = true,
        };
        var studio = new StudioEngineInstaller();
        
        var context = new EngineContext
        {
            Name = "RedHerring Studio",
            Platform = _platform,
            Window = _window!,
        }
            .WithAssemblies(AppDomain.CurrentDomain.GetAssemblies())
            .WithAssembly(typeof(Definitions).Assembly) //TODO(Mirzi): this should not have to be added manually
            .WithInstaller(render)
            .WithInstaller(studio);
        _engine.Run(context);

        _sessionContext = new SessionContext();
        _engine.Run(_sessionContext);
        
        _window!.IsVisible = true;
    }

    private static void OnResize(Vector2D<int> size)
    {
        _engine.Resize(size);
    }

    private static void OnUpdate(double time)
    {
        _engine.Update(time);
    }

    private static void OnDraw(double time)
    {
        _engine.Draw(time);
    }

    private static void OnClose()
    {
        _engine.Exit();
    }

    private static void OnEngineExit()
    {
        Definitions.DestroyContext();
        _window?.Close();
    }

    private static void OnFocusChanged(bool hasFocus)
    {
        _sessionContext?.Container.Resolve<StudioSystem>()?.FocusChanged(hasFocus);
    }

    #endregion Bindings
}