using ImGuiNET;
using RedHerring.Alexandria;
using RedHerring.Infusion.Attributes;
using RedHerring.Inputs;
using RedHerring.Inputs.Layers;
using RedHerring.Render.ImGui;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Core.Systems;

public sealed class InputSystem : EngineSystem, Updatable, Drawable
{
    [Infuse]
    private InputLayer _layer = null!;

    public bool IsEnabled => true;
    public int UpdateOrder => -1_000_000;

    private bool _debugDrawImGuiMetrics = false;

    #region Lifecycle

    protected override void Init()
    {
        AddDebugBindings();
    }
    public void Update(GameTime gameTime)
    {
        UpdateCursor();

        var context = Interaction.CurrentContext();
        if (context is not null)
        {
            ImGuiProxy.Update(gameTime, context);
        }
        
        Interaction.NextFrame();
    }

    #endregion Lifecycle

    #region Manipulation

    public bool AddBinding(ShortcutBinding binding)
    {
        var context = Interaction.CurrentContext()!;
        context.Bindings!.Add(binding);
        return true;
    }

    #endregion Manipulation

    #region Private

    private void UpdateCursor()
    {
        CursorKind cursor = Gui.GetMouseCursor() switch
        {
            ImGuiMouseCursor.Arrow      => CursorKind.Default,
            ImGuiMouseCursor.TextInput  => CursorKind.IBeam,
            ImGuiMouseCursor.ResizeAll  => CursorKind.Default,
            ImGuiMouseCursor.ResizeNS   => CursorKind.ResizeNS,
            ImGuiMouseCursor.ResizeEW   => CursorKind.ResizeWE,
            ImGuiMouseCursor.ResizeNESW => CursorKind.ResizeNESW,
            ImGuiMouseCursor.ResizeNWSE => CursorKind.ResizeNWSE,
            ImGuiMouseCursor.Hand       => CursorKind.Hand,
            ImGuiMouseCursor.NotAllowed => CursorKind.No,
            _                           => CursorKind.Default,
        };

        Interaction.Cursor(cursor);
    }

    #endregion Private

    #region Debug

    private void AddDebugBindings()
    {
        _layer.Name = "input_debug";
            
        var context = Interaction.CurrentContext();
        if (context is null || context.Bindings is null)
        {
            return;
        }
        
        AddBinding(new ShortcutBinding("imgui_metrics", new Shortcut(Input.F11, Modifier.Shift)));
        AddBinding(new ShortcutBinding("dbg_draw", new Shortcut(Input.F12, Modifier.Shift)));
        
        _layer.Bind("imgui_metrics", InputState.Released, ToggleImGuiMetrics);
        _layer.Bind("dbg_draw", InputState.Released, ToggleDebugDraw);
        _layer.Push();
    }
    
    private void ToggleImGuiMetrics(ref ActionEvent evt)
    {
        evt.Consumed = true;

        _debugDrawImGuiMetrics = !_debugDrawImGuiMetrics;
    }

    private void ToggleDebugDraw(ref ActionEvent evt)
    {
        evt.Consumed = true;
        
        var context = Interaction.CurrentContext();
        if (context is null)
        {
            return;
        }
        
        if (context.IsDebugging)
        {
            context.DisableDebug();
        }
        else
        {
            context.EnableDebug();
        }
    }

    #endregion Debug

    #region Drawable

    public bool IsVisible => _debugDrawImGuiMetrics;
    public int DrawOrder => 10_000_000;
    public bool BeginDraw() => _debugDrawImGuiMetrics;

    public void Draw(GameTime gameTime)
    {
        if (_debugDrawImGuiMetrics)
        {
            Gui.ShowMetricsWindow();
        }
    }

    public void EndDraw()
    {
    }

    #endregion Drawable
}