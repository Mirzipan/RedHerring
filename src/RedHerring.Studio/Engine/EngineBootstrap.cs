using System.Runtime.InteropServices;
using Silk.NET.Windowing;
using Veldrid;

namespace RedHerring.Studio.Engine;

public static class EngineBootstrap
{
    public static RedHerring.Engines.Engine Start(IView view)
    {
        var context = new StudioEngineContext
        {
            View = view,
        };

        var engine = new Engines.Engine();
        engine.Run(context);
        return engine;
    }
    
    internal static GraphicsBackend GetPreferredBackend()
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
}