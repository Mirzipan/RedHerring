using RedHerring.Alexandria.Identifiers;
using RedHerring.Core;
using RedHerring.Fingerprint;
using RedHerring.Fingerprint.Layers;
using RedHerring.Infusion.Attributes;
using RedHerring.Render;
using RedHerring.Studio.Constants;
using RedHerring.Studio.Debug;

namespace RedHerring.Studio.Systems;

public class StudioGraphics : EngineSystem
{
    [Infuse]
    private Renderer _renderer = null!;
    [Infuse]
    private InputReceiver _receiver = null!;

    #region Lifecycle

    protected override void Init()
    {
        SetupInput();
        
        // debug
        _renderer.AddFeature(new StudioTestRenderFeature());
    }

    protected override ValueTask<int> Load()
    {        
        _receiver.Push();
        return ValueTask.FromResult(0);
    }

    protected override ValueTask<int> Unload()
    {        
        _receiver.Pop();
        return ValueTask.FromResult(0);
    }

    #endregion Lifecycle

    #region Private

    private void SetupInput()
    {
        _receiver.Name = "studio graphics";
        _receiver.Layer = new OctoByte("graphics");
        _receiver.ConsumesAllInput = false;
        
        _receiver.Bind(InputAction.ReloadShaders, InputState.Released, OnReloadShaders);
    }

    #endregion Private

    #region Bindings

    private void OnReloadShaders(ref ActionEvent evt)
    {
        evt.Consumed = true;
        _renderer.ReloadShaders();
    }

    #endregion Bindings
}