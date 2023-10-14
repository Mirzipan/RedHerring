using System.Globalization;
using RedHerring.Alexandria;
using RedHerring.Render.Components;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Extensions.Veldrid;
using Veldrid;

namespace RedHerring.Render;

public class Renderer : AThingamabob
{
    public readonly Thread Thread;
    
    private GraphicsDevice _graphicsDevice;
    private ResourceFactory _resourceFactory;
    private CommandList _commandList;

    private RendererComponentCollection _components;
    
    // TODO graphics context
    // TODO command list
    // TODO graphics device

    public RendererComponentCollection Components => _components;

    #region Lifecycle

    public Renderer(IView view, GraphicsBackend backend, bool useSeparateThread, string? name = null) : base(name)
    {
        if (useSeparateThread)
        {
            Thread = new Thread(ThreadStart)
            {
                IsBackground = true,
                Name = "Render",
                CurrentCulture = CultureInfo.InvariantCulture,
                CurrentUICulture = CultureInfo.InvariantCulture,
            };
        }
        else
        {
            Thread = Thread.CurrentThread;
        }
        
        _graphicsDevice = view.CreateGraphicsDevice(new GraphicsDeviceOptions
        {
            PreferDepthRangeZeroToOne = true,
            PreferStandardClipSpaceYDirection = true,
        }, backend);

        _resourceFactory = _graphicsDevice.ResourceFactory;
        _commandList = _resourceFactory.CreateCommandList();

        _components = new RendererComponentCollection();
        
        var debug = new DebugRenderComponent(_graphicsDevice, _resourceFactory);
        _components.Add(debug);
    }

    private void ThreadStart(object? obj)
    {
        
    }

    protected override void Destroy()
    {
        _commandList.Dispose();
        _graphicsDevice.Dispose();
    }

    public bool BeginDraw()
    {
        _commandList.Begin();
        _commandList.SetFramebuffer(_graphicsDevice.SwapchainFramebuffer);
        _commandList.ClearColorTarget(0, RgbaFloat.Black);
        
        // TODO reset command list
        // TODO clear states
        // TODO some other render magic
        return true;
    }

    public void Draw()
    {
        _components.Draw(_commandList);
        
        // TODO ensure render targets and other magic
    }

    public void EndDraw()
    {
        _commandList.End();
        
        _graphicsDevice.SubmitCommands(_commandList);
        _graphicsDevice.SwapBuffers();
    }

    #endregion Lifecycle

    #region Public

    public void Resize(Vector2D<int> size)
    {
        _graphicsDevice.ResizeMainWindow((uint)size.X, (uint)size.Y);
    }

    #endregion Public
}