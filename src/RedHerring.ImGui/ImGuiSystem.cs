using ImGuiNET;
using RedHerring.Alexandria;
using RedHerring.Core;
using RedHerring.Core.Systems;
using RedHerring.Fingerprint;
using RedHerring.Fingerprint.Layers;
using RedHerring.Fingerprint.Shortcuts;
using RedHerring.Infusion.Attributes;
using Silk.NET.Input;
using Gui = ImGuiNET.ImGui;
using Key = RedHerring.Fingerprint.Key;

namespace RedHerring.ImGui;

public class ImGuiSystem : EngineSystem, Updatable, Drawable
{
    [Infuse]
    private InputSystem _inputSystem = null!;
    [Infuse]
    private GraphicsSystem _graphicsSystem = null!;

    [Infuse]
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

        CreateShortcuts();
        // SubscribeToInput();
    }

    protected override ValueTask<int> Load()
    {
        _feature = new ImGuiRenderFeature();
        _graphicsSystem.RegisterFeature(_feature);
        return ValueTask.FromResult(0);
    }

    protected override ValueTask<int> Unload()
    {
        // UnsubscribeFromInput();
        return ValueTask.FromResult(0);
    }
    
    public void Update(GameTime gameTime)
    {
        UpdateCursor();
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

    #region Private

    private void CreateShortcuts()
    {
        _inputSystem.AddBinding("imgui_metrics", new KeyboardShortcut(Key.F11, Modifiers.Shift));

        _receiver.Name = "ImGui";
        _receiver.Bind("imgui_metrics", InputState.Released, ToggleFontDebug);
        _receiver.Push();
    }

    private void UpdateCursor()
    {
        StandardCursor cursor = Gui.GetMouseCursor() switch
        {
            ImGuiMouseCursor.None       => StandardCursor.Default,
            ImGuiMouseCursor.Arrow      => StandardCursor.Default,
            ImGuiMouseCursor.TextInput  => StandardCursor.IBeam,
            ImGuiMouseCursor.ResizeAll  => StandardCursor.Default,
            ImGuiMouseCursor.ResizeNS   => StandardCursor.VResize,
            ImGuiMouseCursor.ResizeEW   => StandardCursor.HResize,
            ImGuiMouseCursor.ResizeNESW => StandardCursor.Default,
            ImGuiMouseCursor.ResizeNWSE => StandardCursor.Default,
            ImGuiMouseCursor.Hand       => StandardCursor.Hand,
            ImGuiMouseCursor.NotAllowed => StandardCursor.Default,
            ImGuiMouseCursor.COUNT      => StandardCursor.Default,
            _                           => throw new ArgumentOutOfRangeException()
        };

        if (_inputSystem.Mouse != null)
        {
            _inputSystem.Mouse.Mouse.Cursor.StandardCursor = cursor;
        }
    }
    
    #endregion Private

    #region Bindings
    
    private void ToggleFontDebug(ref ActionEvent evt)
    {
        evt.Consumed = true;

        _debugDraw = !_debugDraw;
    }

    #endregion Bindings
}