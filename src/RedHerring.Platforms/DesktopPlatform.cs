using System.Reflection;
using System.Runtime.InteropServices;
using RedHerring.Alexandria.Extensions.Collections;
using RedHerring.Core;
using RedHerring.Game;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Extensions.Veldrid;
using Veldrid;

namespace RedHerring.Platforms;

public sealed class DesktopPlatform : Platform
{
    // TODO: platform should manage the window and update the engine, so that the Program will only need to create a platform
    private IWindow? _window;
    private Engine _engine;
    private EngineContext _engineContext = null!;
    
    private string _applicationDataDirectory = null!;
    private string _applicationDirectory = null!;
    private string _resourcesDirectory = null!;

    public Engine Engine => _engine;
    public GraphicsBackend GraphicsBackend { get; init; }
    public IWindow? MainWindow => _window;

    public string ApplicationDataDirectory => _applicationDataDirectory;
    public string ApplicationDirectory => _applicationDirectory;
    public string ResourcesDirectory => _resourcesDirectory;

    #region Lifecycle

    public DesktopPlatform(EngineContext context)
    {
        InitDirectories(context.ProductName);
        GraphicsBackend = PreferredGraphicsBackend();
        
        _engineContext = context;
        
        _engine = new Engine();
        _engine.OnExit += OnEngineExit;
    }

    #endregion Lifecycle

    #region Public
    
    public void Run()
    {
        _window = CreateWindow();
        
        _window.Load += OnLoad;
        _window.Update += OnUpdate;
        _window.Render += OnDraw;
        _window.Closing += OnClose;
        _window.Resize += OnResize;

        _window.Run();
    }

    public void Run(SessionContext context)
    {
    }
    
    public IWindow CreateWindow()
    {
        var opts = new WindowOptions
        {
            Title = _engineContext.ProductName,
            Position = new Vector2D<int>(100, 100),
            Size = new Vector2D<int>(_engineContext.DesiredWidth, _engineContext.DesiredHeight),
            API = GraphicsBackend.ToGraphicsAPI(),
            VSync = true,
            ShouldSwapAutomatically = false,
        };
        
        return Window.Create(opts);
    }

    #endregion Public

    #region Private

    private void InitDirectories(string name)
    {
        var uri = new Uri(Assembly.GetEntryAssembly()!.Location);
        _applicationDirectory = uri.LocalPath;
        _resourcesDirectory = Path.Combine(_applicationDirectory, "Resources");
        
        string homePath = HomePath();
        _applicationDataDirectory = Path.Combine(homePath, name);
    }

    #endregion Private

    #region Queries

    private static GraphicsBackend PreferredGraphicsBackend()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return GraphicsDevice.IsBackendSupported(GraphicsBackend.Vulkan)
                ? GraphicsBackend.Vulkan
                : GraphicsBackend.Direct3D11;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return GraphicsDevice.IsBackendSupported(GraphicsBackend.Metal)
                ? GraphicsBackend.Metal
                : GraphicsBackend.OpenGL;
        }

        return GraphicsDevice.IsBackendSupported(GraphicsBackend.Vulkan)
            ? GraphicsBackend.Vulkan
            : GraphicsBackend.OpenGL;
    }

    private static string HomePath()
    {
        string? path = null;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            path = Environment.ExpandEnvironmentVariables("%APPDATA%");
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            path = Environment.ExpandEnvironmentVariables("XDG_CONFIG_HOME");

            if (path.IsNullOrEmpty())
            {
                path = Environment.ExpandEnvironmentVariables("~/Library/Application Support");
            }
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            path = Environment.ExpandEnvironmentVariables("XDG_CONFIG_HOME");
        }

        if (path.IsNullOrEmpty())
        {
            path = "./";
        }

        return path!;
    }

    #endregion Queries

    #region Bindings

    private void OnResize(Vector2D<int> size)
    {
        _engine.Resize(size);
    }

    private void OnDraw(double time)
    {
        _engine.Update(time);
    }

    private void OnUpdate(double time)
    {
        _engine.Draw(time);
    }

    private void OnLoad()
    {
        _engine.Run(_engineContext);
        _window!.IsVisible = true;
    }

    private void OnEngineExit()
    {
        _window?.Close();
    }

    private void OnClose()
    {
        _engine.Exit();
    }

    #endregion Bindings
}