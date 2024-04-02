using System.Runtime.InteropServices;
using RedHerring.Infusion;
using Silk.NET.Windowing;
using Veldrid;

namespace RedHerring.Render;

public class RenderInstaller : BindingsInstaller
{
    private IView? _view;
    public GraphicsBackend Backend;
    public bool UseSeparateRenderThread;

    public RenderInstaller(IView? view)
    {
        _view = view;
    }

    public void InstallBindings(ContainerDescription description)
    {
        RenderDevice device;
        
        if (_view is not null)
        {
            device = new UniversalRenderDevice(_view, Backend);
        }
        else
        {
            device = new NullRenderDevice();
        }
        
        var context = Renderer.Init(device);
        description.AddSingleton(context, typeof(RenderContext));
    }
    
    public static GraphicsBackend PreferredBackend()
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