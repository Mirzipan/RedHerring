﻿using System.Reactive.Disposables;
using RedHerring.Alexandria.Components;
using RedHerring.Alexandria.Disposables;
using RedHerring.Render.Passes;
using Silk.NET.Maths;
using Veldrid;

namespace RedHerring.Render.Features;

public abstract class RenderFeature : Component<RenderFeatureCollection>, IDisposerContainer, IDisposable
{
    private bool _initialized;
    private RenderFeatureCollection? _container;
    public override RenderFeatureCollection? Container => _container;
    CompositeDisposable IDisposerContainer.Disposer { get; set; } = new();
    public abstract int Priority { get; }
    public bool Initialized => _initialized;

    internal void RaiseInit(GraphicsDevice device, CommandList commandList)
    {
        Init(device, commandList);
        _initialized = true;
    }

    internal void RaiseUnload(GraphicsDevice device, CommandList commandList)
    {
        Unload(device, commandList);
        _initialized = false;
    }

    protected abstract void Init(GraphicsDevice device, CommandList commandList);
    protected abstract void Unload(GraphicsDevice device, CommandList commandList);
    
    public abstract void Update(GraphicsDevice device, CommandList commandList);
    
    public abstract void Render(GraphicsDevice device, CommandList commandList, RenderEnvironment environment, RenderPass pass);

    public abstract void Resize(Vector2D<int> size);
    
    public void Dispose()
    {
        _container?.Dispose();
        ((IDisposerContainer)this).Disposer.Dispose();
    }

    internal void SetContainer(RenderFeatureCollection container)
    {
        _container = container;
    }

    protected void ResetDisposer()
    {
        var container = (IDisposerContainer)this;
        container.Disposer.Dispose();
        container.Disposer = new CompositeDisposable();
    }
}