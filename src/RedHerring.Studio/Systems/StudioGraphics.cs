using RedHerring.Alexandria.Identifiers;
using RedHerring.Core;
using RedHerring.Infusion.Attributes;
using RedHerring.Inputs;
using RedHerring.Inputs.Layers;
using RedHerring.Render;
using RedHerring.Studio.Constants;
using RedHerring.Studio.Debug;

namespace RedHerring.Studio.Systems;

public class StudioGraphics : EngineSystem
{
    [Infuse]
    private RendererContext _rendererContext = null!;
    [Infuse]
    private InputLayer _layer = null!;

    #region Lifecycle

    protected override void Init()
    {
        SetupInput();
        
        // debug
        _rendererContext.AddFeature(new StudioTestRenderFeature());
    }

    protected override ValueTask<int> Load()
    {        
        _layer.Push();
        return ValueTask.FromResult(0);
    }

    protected override ValueTask<int> Unload()
    {        
        _layer.Pop();
        return ValueTask.FromResult(0);
    }

    #endregion Lifecycle

    #region Private

    private void SetupInput()
    {
        _layer.Name = "studio graphics";
        _layer.Layer = "graphics";
        _layer.ConsumesAllInput = false;
        
        _layer.Bind(InputAction.ReloadShaders, InputState.Released, OnReloadShaders);
    }

    #endregion Private

    #region Bindings

    private void OnReloadShaders(ref ActionEvent evt)
    {
        evt.Consumed = true;
        _rendererContext.ReloadShaders();
    }

    #endregion Bindings
}