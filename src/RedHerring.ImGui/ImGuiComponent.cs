using System.Numerics;
using ImGuiNET;
using RedHerring.Alexandria;
using RedHerring.Core;
using RedHerring.Core.Components;
using RedHerring.Infusion.Attributes;
using Veldrid;
using Vortice.Mathematics;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.ImGui;

public class ImGuiComponent : AnEngineComponent, IDrawable
{
    private const ImGuiWindowFlags BackgroundWindowFlags = ImGuiWindowFlags.NoSavedSettings 
                                                 | ImGuiWindowFlags.NoCollapse 
                                                 | ImGuiWindowFlags.NoTitleBar 
                                                 | ImGuiWindowFlags.NoResize 
                                                 | ImGuiWindowFlags.NoScrollbar 
                                                 | ImGuiWindowFlags.NoMove 
                                                 | ImGuiWindowFlags.NoBackground;
    
    [Inject]
    private InputComponent _inputComponent = null!;
    [Inject]
    private GraphicsComponent _graphicsComponent = null!;

    private RedInputSnapshot _inputSnapshot = null!;
    
    private ImGuiRenderer? _renderer;
    private byte[] _openSansData = null!;
    private ImFontConfigPtr _openSansConfig;
    
    public bool IsVisible => true;
    public int DrawOrder => 10_000_000;

    #region Lifecycle

    protected override void Init()
    {
    }

    protected override void Load()
    {
        _inputSnapshot = new RedInputSnapshot(_inputComponent.Input);

        LoadDefaultFontData();
        ResetRenderer();
    }

    protected override void Unload()
    {
        FontLoader.Unload();

        if (_renderer is not null)
        {
            _renderer.ClearCachedImageResources();
            _renderer.DestroyDeviceObjects();
            _renderer.Dispose();
        }
        
        FontLoader.Unloaded(_openSansConfig);
    }

    #endregion Lifecycle

    #region Drawable

    public bool BeginDraw()
    {
        return true;
    }

    public void Draw(GameTime gameTime)
    {
        _inputSnapshot.Update();
        _renderer?.Update(gameTime.Elapsed, _inputSnapshot);
        
        Gui.SetNextWindowPos(Vector2.Zero);
        Gui.SetNextWindowSize((Vector2)Context.View.Size);
        Gui.Begin("Canvas", BackgroundWindowFlags);

        Gui.TextColored(Color3.AliceBlue.ToVector4(), "Debug");
        DrawDeviceInfo();
        
        Gui.End();
    }

    public void EndDraw()
    {
        var device = _graphicsComponent.Device;
        var cl = _graphicsComponent.CommandList;
        
        _renderer?.Render(device, cl);
    }

    #endregion Drawable

    #region Private

    private void ResetRenderer()
    {
        if (_renderer is not null)
        {
            _renderer.ClearCachedImageResources();
            _renderer.DestroyDeviceObjects();
            _renderer.Dispose();
        }


        var size = Context.View.FramebufferSize;
        _renderer = new ImGuiRenderer(
            _graphicsComponent.Device,
            _graphicsComponent.Device.MainSwapchain.Framebuffer.OutputDescription,
            size.X,
            size.Y);

        RecreateFont();
    }
    
    private void LoadDefaultFontData()
    {
        _openSansConfig = FontLoader.LoadFontData(Base64Font.OpenSans, out _openSansData);
    }

    private void RecreateFont()
    {
        FontLoader.RecreateFont(_renderer!, _openSansData, _openSansConfig);
    }

    private void DrawDeviceInfo()
    {
        var device = _graphicsComponent.Device;
        
        Gui.Text("Device Info");
        AddRow("Name:", device.DeviceName);
        AddRow("Backend:", device.BackendType.ToString());
        AddRow("API Version:", device.ApiVersion.ToString()!);
    }

    private static void AddRow(string column1, string column2)
    {
        Gui.TextColored(Color3.LightGray.ToVector4(), column1);
        Gui.SameLine();
        Gui.Text(column2);
    }

    #endregion Private
}