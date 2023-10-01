using Silk.NET.Windowing;
using Veldrid;

namespace RedHerring.Engines;

/// <summary>
/// Context used for running the engine.
/// </summary>
public abstract class AnEngineContext
{
    public IView View { get; set; } = null!;
    public GraphicsBackend GraphicsBackend { get; set; }
}