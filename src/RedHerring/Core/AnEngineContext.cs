﻿using RedHerring.Core.Systems;
using Silk.NET.Windowing;
using Veldrid;

namespace RedHerring.Core;

/// <summary>
/// Context used for running the engine.
/// </summary>
public abstract class AnEngineContext
{
    public IView View { get; set; } = null!;
    public GraphicsBackend GraphicsBackend { get; set; }
    public List<SystemReference> Systems { get; set; } = new();
    public bool UseSeparateRenderThread { get; set; }
}