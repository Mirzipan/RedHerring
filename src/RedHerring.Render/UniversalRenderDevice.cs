using RedHerring.Alexandria;
using RedHerring.Alexandria.Disposables;
using RedHerring.Render.Features;
using RedHerring.Render.ImGui;
using RedHerring.Render.Passes;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Extensions.Veldrid;
using Veldrid;
using ImGuiRenderer = RedHerring.Render.ImGui.ImGuiRenderer;

namespace RedHerring.Render;

public sealed class UniversalRenderDevice : NamedDisposer, RenderDevice
{
    private GraphicsDevice _graphicsDevice;
    private ResourceFactory _resourceFactory;
    private CommandList _commandList;
    private ImGuiRenderer? _imGui;
    
    private Vector2D<int> _size;
    
    public GraphicsDevice Device => _graphicsDevice;
    public ResourceFactory ResourceFactory => _resourceFactory;
    public CommandList CommandList => _commandList;

    // TODO graphics context

    #region Lifecycle

    public UniversalRenderDevice(IView view, GraphicsBackend backend, string? name = null) : base(name)
    {
        _graphicsDevice = view.CreateGraphicsDevice(new GraphicsDeviceOptions
        {
            PreferDepthRangeZeroToOne         = true,
            PreferStandardClipSpaceYDirection = true,
            SwapchainDepthFormat              = PixelFormat.D32_Float_S8_UInt, // PixelFormat.D24_UNorm_S8_UInt,
            ResourceBindingModel = ResourceBindingModel.Improved,
        }, backend);

        _resourceFactory = _graphicsDevice.ResourceFactory;
        _commandList = _resourceFactory.CreateCommandList();
        
        //var debug = new DebugRenderFeature();
        //_features.Add(debug);
    }

    public void Init(RenderFeatureCollection features)
    {
        features.Init(_graphicsDevice, _commandList);
    }

    public void Init(RenderFeature feature)
    {
        feature.RaiseInit(_graphicsDevice, _commandList);
    }

    public void ReloadShaders(RenderFeatureCollection features)
    {
        features.ReloadShaders(_graphicsDevice, _commandList);
    }

    public void Init()
    {
        ImGuiProxy.ResetImGuiRenderer(ref _imGui, _graphicsDevice, _size.X, _size.Y);
        _imGui?.DisposeWith(this);
    }

    public void Close()
    {
    }

    protected override void Destroy()
    {
        _commandList.Dispose();
        _graphicsDevice.Dispose();
        
        _imGui?.Dispose();
        ImGuiProxy.Dispose();
    }

    public bool BeginDraw()
    {
        _commandList.Begin();
        _commandList.SetFramebuffer(_graphicsDevice.SwapchainFramebuffer);
        _commandList.ClearColorTarget(0, RgbaFloat.Black);
        _commandList.ClearDepthStencil(1f, 0);
        
        // TODO reset command list
        // TODO clear states
        // TODO some other render magic
        return true;
    }

    public void Draw(RenderContext context)
    {
        context.Features.Update(_graphicsDevice, _commandList);
        context.Features.Render(_graphicsDevice, _commandList, context.Environment, new RenderPass());
        
        // TODO ensure render targets and other magic
        
        _imGui?.Render(_graphicsDevice, _commandList);
    }

    public void EndDraw()
    {
        _commandList.End();
        
        _graphicsDevice.SubmitCommands(_commandList);
        _graphicsDevice.WaitForIdle();
        _graphicsDevice.SwapBuffers();
    }

    #endregion Lifecycle

    #region Public

    public void Resize(Vector2D<int> size)
    {
        _size = size;
        _graphicsDevice.ResizeMainWindow((uint)size.X, (uint)size.Y);
        _imGui?.WindowResized(size.X, size.Y);
    }

    #endregion Public
}