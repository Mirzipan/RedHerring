using GameLibrary;
using RedHerring.Core;
using RedHerring.Game;
using RedHerring.Platforms;
using RedHerring.Render;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Extensions.Veldrid;
using Veldrid;

namespace Template;

internal class Program
{
    private static IWindow? _window;

    private static Engine _engine = null!;
    private static SessionContext _sessionContext = null!;

    private static GraphicsBackend _graphicsBackend;

    private static Platform _platform = null!;

    private static void Main(string[] args)
    {
        _platform = DesktopPlatform.Create("Template");
        
        Init();
        
        var opts = new WindowOptions
        {
            Title = "Template",
            Position = new Vector2D<int>(100, 100),
            Size = new Vector2D<int>(960, 540),
            API = _graphicsBackend.ToGraphicsAPI(),
            VSync = true,
            ShouldSwapAutomatically = false,
        };
        
        _window = Window.Create(opts);
        
        _window.Load += OnLoad;
        _window.Update += OnUpdate;
        _window.Render += OnDraw;
        _window.Closing += OnClose;
        _window.Resize += OnResize;

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
        var gameEngineInstaller = new TemplateEngineInstaller();
        
        var context = new EngineContext
        {
            Name = "Template",
            Platform = _platform,
            Window = _window!,
        }.WithAssemblies(AppDomain.CurrentDomain.GetAssemblies()).WithInstaller(render).WithInstaller(gameEngineInstaller);
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
        _window?.Close();
    }

    #endregion Bindings
}