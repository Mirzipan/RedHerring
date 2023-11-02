using RedHerring.Alexandria;
using RedHerring.Render.Features;
using RedHerring.Render.Passes;
using Silk.NET.Maths;
using Veldrid;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.ImGui;

public class ImGuiRenderFeature : ARenderFeature
{
    private ImGuiRenderer? _renderer;
    
    public override int Priority { get; } = -1_000_000;
    public Vector2D<int> Size { get; set; }

    #region Lifecycle

    public override void Init(GraphicsDevice device, CommandList commandList)
    {
        ResetRenderer(device);
    }

    public override void Update(GraphicsDevice device, CommandList commandList)
    {
    }

    public override void Render(GraphicsDevice device, CommandList commandList, RenderPass pass)
    {
        _renderer?.Render(device, commandList);
    }

    public override void Resize(Vector2D<int> size)
    {
        Size = size;
        _renderer?.WindowResized(size.X, size.Y);
    }

    public override void Destroy()
    {
        FontLoader.Unload();

        if (_renderer is not null)
        {
            _renderer.ClearCachedImageResources();
            _renderer.DestroyDeviceObjects();
            _renderer.Dispose();
        }
        
        //FontLoader.Unloaded(_openSansConfig);
    }

    #endregion Lifecycle

    #region Public

    public void Update(GameTime time, InputSnapshot snapshot)
    {
        _renderer?.Update((float)time.Elapsed.TotalSeconds, snapshot);
    }

    #endregion Public

    #region Private

    private void ResetRenderer(GraphicsDevice device)
    {
        if (_renderer is not null)
        {
            _renderer.ClearCachedImageResources();
            _renderer.DestroyDeviceObjects();
            _renderer.Dispose();
        }


        var size = Size;
        _renderer = new ImGuiRenderer(
            device,
            device.MainSwapchain.Framebuffer.OutputDescription,
            size.X,
            size.Y);

        RecreateFont();
        Theme.CrimsonRivers();
    }

    private void RecreateFont()
    {
        FontLoader.Load(_renderer!);
    }

    #endregion Private
}