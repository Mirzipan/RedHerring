using RedHerring.Alexandria.Identifiers;
using RedHerring.Core;
using RedHerring.Fingerprint;
using RedHerring.Fingerprint.Layers;
using RedHerring.Infusion.Attributes;
using RedHerring.Render;
using RedHerring.Studio.Constants;

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
        
        _receiver.Bind(InputAction.ResetRenderer, InputState.Released, OnResetRenderer);
    }

    #endregion Private

    #region Bindings

    private void OnResetRenderer(ref ActionEvent evt)
    {
        evt.Consumed = true;
        _renderer.Reset();
    }

    #endregion Bindings
}