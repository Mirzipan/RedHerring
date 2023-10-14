using RedHerring.Engines;
using RedHerring.Motive.Games;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Extensions.Veldrid;
using Veldrid;

namespace RedHerring.Examples;

internal class Program
{
    private const GraphicsBackend PreferredBackend = GraphicsBackend.Vulkan;

    private static IWindow? _window;

    private static ExampleEngineContext _engineContext = null!;
    private static Engine _engine = null!;

    private static ExampleGameContext _gameContext = null!;
    private static Game _game = null!;

    private static void Main(string[] args)
    {
        var opts = new WindowOptions
        {
            Title = "Red Herring Engine Example",
            Position = new Vector2D<int>(100, 100),
            Size = new Vector2D<int>(960, 540),
            API = PreferredBackend.ToGraphicsAPI(),
            VSync = false,
            ShouldSwapAutomatically = false,
        };
        
        _window = Window.Create(opts);
        
        _window.Load += OnLoad;
        _window.Render += OnDraw;
        _window.Closing += OnClose;
        _window.Resize += OnResize;

        _window.Run();
    }

    #region Bindings

    static void OnLoad()
    {
        _engine = new Engine();

        _engineContext = new ExampleEngineContext
        {
            View = _window!,
            GraphicsBackend = PreferredBackend,
        };
        _engine.Run(_engineContext);

        _gameContext = new ExampleGameContext();
        _game = new Game(_gameContext);
        _engine.Run(_game);
        
        _window!.IsVisible = true;
    }

    private static void OnResize(Vector2D<int> size)
    {
        _engine.Resize(size);
    }

    private static void OnDraw(double time)
    {
        _engine.Tick();
    }

    private static void OnClose()
    {
        _engine.Exit();
    }

    #endregion Bindings
}