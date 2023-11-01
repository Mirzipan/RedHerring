using RedHerring.Alexandria;
using RedHerring.Core;
using RedHerring.Core.Systems;
using RedHerring.Fingerprint;
using RedHerring.Fingerprint.Layers;
using RedHerring.Fingerprint.Shortcuts;
using RedHerring.Infusion.Attributes;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.ImGui;

public class ImGuiSystem : AnEngineSystem, IUpdatable, IDrawable
{
    [Inject]
    private InputSystem _inputSystem = null!;
    [Inject]
    private GraphicsSystem _graphicsSystem = null!;

    [Inject]
    private InputReceiver _receiver = null!;

    private ImInputSnapshot _imInputSnapshot = null!;
    private ImGuiRenderFeature _feature = null!;
    
    public bool IsVisible => true;
    public int DrawOrder => 10_000_000;

    public bool IsEnabled => true;
    public int UpdateOrder => -10_000_000;

    private bool _debugDraw;

    #region Lifecycle

    protected override void Init()
    {
        _imInputSnapshot = new ImInputSnapshot();
        
        var shortcut = new CompositeShortcut();
        shortcut.Add(new KeyboardShortcut(Key.ShiftLeft));
        shortcut.Add(new KeyboardShortcut(Key.F11));
        _inputSystem.AddBinding("imgui_metrics", shortcut);
        
        _receiver.Name = "ImGui";
        _receiver.Bind("imgui_metrics", InputState.Released, ToggleFontDebug);
        _receiver.Push();
    }

    protected override ValueTask<int> Load()
    {
        _feature = new ImGuiRenderFeature();
        _graphicsSystem.RegisterFeature(_feature);
        return ValueTask.FromResult(0);
    }

    protected override ValueTask<int> Unload()
    {
        return ValueTask.FromResult(0);
    }
    
    public void Update(GameTime gameTime)
    {
        _imInputSnapshot.Update(_inputSystem.Input);
        _feature.Update(gameTime, _imInputSnapshot);
    }

    #endregion Lifecycle

    #region Drawable

    public bool BeginDraw()
    {
        return true;
    }

    public void Draw(GameTime gameTime)
    {
        if (_debugDraw)
        {
            Gui.ShowMetricsWindow();
        }
    }

    public void EndDraw()
    {
    }

    #endregion Drawable

    #region Bindings
    
    private void ToggleFontDebug(ref ActionEvent evt)
    {
        evt.Consumed = true;

        _debugDraw = !_debugDraw;
    }

    #endregion Bindings
}