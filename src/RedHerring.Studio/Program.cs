using RedHerring.Core;
using RedHerring.Game;
using RedHerring.Platforms;
using RedHerring.Render;
using Veldrid;

namespace RedHerring.Studio;

internal class Program
{
    private static Engine _engine = null!;
    private static SessionContext _sessionContext = null!;

    private static GraphicsBackend _graphicsBackend;

    private static Platform _platform = null!;

    private static void Main(string[] args)
    {
        var context = EngineContext();
        _platform = new DesktopPlatform(context);
        _platform.Run();
    }

    #region Private

    private static EngineContext EngineContext()
    {
        var render = new RenderInstaller(_window!)
        {
            Backend = _graphicsBackend,
            UseSeparateRenderThread = true,
        };
        var studio = new StudioEngineInstaller();
        
        var context = new EngineContext
        {
            Name = "RedHerring Studio",
            DesiredWidth = 1280,
            DesiredHeight = 720,
        }.WithAssemblies(AppDomain.CurrentDomain.GetAssemblies()).WithInstaller(render).WithInstaller(studio);

        return context;
    }
    
    #endregion Private
}