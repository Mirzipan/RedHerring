﻿using System.Runtime.InteropServices;
using RedHerring.Core;
using RedHerring.Game;
using RedHerring.Infusion;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Extensions.Veldrid;
using Veldrid;

namespace RedHerring.Examples;

internal class Program
{
    private static IWindow? _window;
    private static Engine _engine = null!;

    private static SessionContext _sessionContext = null!;

    private static GraphicsBackend _graphicsBackend;

    private static void Main(string[] args)
    {
        Init();
        
        var opts = new WindowOptions
        {
            Title = "Red Herring Engine Example",
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
        _graphicsBackend = GetPreferredBackend();
    }

    private static GraphicsBackend GetPreferredBackend()
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

    #endregion Private

    #region Bindings

    static void OnLoad()
    {
        _engine = new Engine();
        _engine.OnExit += OnEngineExit;

        var context = new EngineContext
        {
            Name = "Game Engine",
            View = _window!,
            GraphicsBackend = _graphicsBackend,
        }.WithAssemblies(AppDomain.CurrentDomain.GetAssemblies()).WithInstaller(new ExampleEngineInstaller());
        
        _engine.Run(context);

        _sessionContext = new SessionContext().WithInstaller(new ExampleSessionInstaller());
        
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