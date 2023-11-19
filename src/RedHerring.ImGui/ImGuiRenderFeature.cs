﻿using RedHerring.Alexandria;
using RedHerring.Alexandria.Disposables;
using RedHerring.Render;
using RedHerring.Render.Features;
using RedHerring.Render.Passes;
using Silk.NET.Maths;
using Veldrid;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.ImGui;

public class ImGuiRenderFeature : RenderFeature
{
    private ImGuiRenderer? _renderer;
    
    public override int Priority { get; } = -1_000_000;
    public Vector2D<int> Size { get; set; }

    #region Lifecycle

    protected override void Init(GraphicsDevice device, CommandList commandList)
    {
        ResetRenderer(device);
    }

    protected override void Unload(GraphicsDevice device, CommandList commandList)
    {
        FontLoader.Unload();

        if (_renderer is not null)
        {
            _renderer.ClearCachedImageResources();
            _renderer.DestroyDeviceObjects();
        }
        
        ResetDisposer();
    }

    public override void Update(GraphicsDevice device, CommandList commandList)
    {
    }

    public override void Render(GraphicsDevice device, CommandList commandList, RenderEnvironment environment, RenderPass pass)
    {
        _renderer?.Render(device, commandList);
    }

    public override void Resize(Vector2D<int> size)
    {
        Size = size;
        _renderer?.WindowResized(size.X, size.Y);
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
        _renderer.DisposeWith(this);

        RecreateFont();
        Theme.CrimsonRivers();
    }

    private void RecreateFont()
    {
        FontLoader.Load(_renderer!);
    }

    #endregion Private
}