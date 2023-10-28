﻿using System.Globalization;
using RedHerring.Alexandria;
using RedHerring.Render.Features;
using RedHerring.Render.Passes;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Extensions.Veldrid;
using Veldrid;

namespace RedHerring.Render;

public class Renderer : ANamedDisposer
{
    public readonly Thread Thread;
    
    private GraphicsDevice _graphicsDevice;
    private ResourceFactory _resourceFactory;
    private CommandList _commandList;

    private RenderFeatureCollection _features;

    public GraphicsDevice Device => _graphicsDevice;
    public CommandList CommandList => _commandList;
    public RenderFeatureCollection Features => _features;
    
    // TODO graphics context

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

        _features = new RenderFeatureCollection();
        
        var debug = new DebugRenderFeature();
        _features.Add(debug);
    }

    public void Init()
    {
        InitFeatures();
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
        _features.Update(_graphicsDevice, _commandList);
        _features.Render(_graphicsDevice, _commandList, new RenderPass());
        
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
        _features.Resize(size);
    }

    #endregion Public

    #region Private

    private void InitFeatures()
    {
        _features.Init(_graphicsDevice, _commandList);
    }

    #endregion Private
}