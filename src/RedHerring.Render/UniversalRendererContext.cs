using System.Globalization;
using System.Numerics;
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

public sealed class UniversalRendererContext : NamedDisposer, RendererContext
{
    public readonly Thread Thread;
    
    private GraphicsDevice _graphicsDevice;
    private ResourceFactory _resourceFactory;
    private CommandList _commandList;

    private RenderFeatureCollection _features;
    private RenderEnvironment _environment = new();
    private ImGuiRenderer? _imGui;
    private Shared _shared = new();
    
    private Vector2D<int> _size;
    
    public GraphicsDevice Device => _graphicsDevice;
    public CommandList CommandList => _commandList;
    public RenderFeatureCollection Features => _features;
    public Shared Shared => _shared; 

    // TODO graphics context

    #region Lifecycle

    public UniversalRendererContext(IView view, GraphicsBackend backend, bool useSeparateThread, string? name = null) : base(name)
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
            PreferDepthRangeZeroToOne         = true,
            PreferStandardClipSpaceYDirection = true,
            SwapchainDepthFormat              = PixelFormat.D24_UNorm_S8_UInt
        }, backend);

        _resourceFactory = _graphicsDevice.ResourceFactory;
        _commandList = _resourceFactory.CreateCommandList();

        _features = new RenderFeatureCollection();
        
        //var debug = new DebugRenderFeature();
        //_features.Add(debug);
    }

    public void AddFeature(RenderFeature feature)
    {
        if (Features.Get(feature.GetType()) is not null)
        {
            return;
        }
        
        Features.Add(feature);
        feature.RaiseInit(_graphicsDevice, _commandList);
        feature.Resize(_size);
    }

    public void Init()
    {
        ImGuiProxy.ResetImGuiRenderer(ref _imGui, _graphicsDevice, _size.X, _size.Y);
        _imGui?.DisposeWith(this);
        
        InitFeatures();
    }

    public void Close()
    {
    }

    private void ThreadStart(object? obj)
    {
        
    }

    protected override void Destroy()
    {
        _shared.Dispose();
        
        _features.ReloadShaders(_graphicsDevice, _commandList);
        _features.Dispose();
        
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

    public void Draw()
    {
        _features.Update(_graphicsDevice, _commandList);
        _features.Render(_graphicsDevice, _commandList, _environment, new RenderPass());
        
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
        _features.Resize(size);
    }

    public void SetCameraViewMatrix(Matrix4x4 world, Matrix4x4 view, Matrix4x4 projection, float fieldOfView, float clipPlaneNear,
        float clipPlaneFar)
    {
        _environment.Position = world.Translation;
        _environment.ViewMatrix = view;
        _environment.ProjectiomMatrix = projection;
        _environment.ViewProjectionMatrix = view * projection;
        _environment.FieldOfView = fieldOfView;
        _environment.ClipPlaneNear = clipPlaneNear;
        _environment.ClipPlaneFar = clipPlaneFar;
    }

    public void ReloadShaders()
    {
        Console.WriteLine("[Renderer] Shader reload - START");
        
        _features.ReloadShaders(_graphicsDevice, _commandList);
        
        Console.WriteLine("[Renderer] Shader reload - DONE");
    }

    #endregion Public

    #region Private

    private void InitFeatures()
    {
        _features.Init(_graphicsDevice, _commandList);
    }

    #endregion Private
}