using System.Numerics;
using ImGuiNET;
using RedHerring.Alexandria;
using RedHerring.Fingerprint;
using RedHerring.Fingerprint.Layers;
using RedHerring.Fingerprint.Shortcuts;
using RedHerring.Fingerprint.States;
using RedHerring.Infusion.Attributes;
using RedHerring.Render.ImGui;
using Silk.NET.Input;
using Key = RedHerring.Fingerprint.Key;
using MouseButton = RedHerring.Fingerprint.MouseButton;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Core.Systems;

public sealed class InputSystem : EngineSystem, Updatable, Drawable
{
    [Infuse]
    private InteractionContext _interactionContext = null!;
    [Infuse]
    private InputReceiver _receiver = null!;

    public bool IsEnabled => true;
    public int UpdateOrder => -1_000_000;

    public InteractionContext InteractionContext => _interactionContext;
    public KeyboardState? Keyboard => _interactionContext.Keyboard;
    public MouseState? Mouse => _interactionContext.Mouse;

    private bool _debugDrawImGuiMetrics = false;

    #region Lifecycle

    protected override void Init()
    {
        AddDebugBindings();
    }
    public void Update(GameTime gameTime)
    {
        UpdateCursor();
        ImGuiProxy.Update(gameTime, _interactionContext);
        
        _interactionContext.Tick();
    }

    #endregion Lifecycle

    #region Queries

    public bool IsKeyPressed(Key key) => _interactionContext.IsKeyPressed(key);
    public bool IsKeyDown(Key key) => _interactionContext.IsKeyDown(key);
    public bool IsKeyReleased(Key key) => _interactionContext.IsKeyReleased(key);
    public bool IsAnyKeyDown() => _interactionContext.IsAnyKeyDown();
    public void GetKeysDown(IList<Key> keys) => _interactionContext.KeysDown(keys);
    
    public bool IsButtonPressed(MouseButton button) => _interactionContext.IsButtonPressed(button);
    public bool IsButtonDown(MouseButton button) => _interactionContext.IsButtonDown(button);
    public bool IsButtonReleased(MouseButton button) => _interactionContext.IsButtonReleased(button);
    public bool IsAnyMouseButtonDown() => _interactionContext.IsAnyMouseButtonDown();
    public void GetButtonsDown(IList<MouseButton> buttons) => _interactionContext.ButtonsDown(buttons);
    public bool IsMouseMoved(MouseAxis axis) => _interactionContext.IsMouseMoved(axis);
    public float GetAxis(MouseAxis axis) => _interactionContext.Axis(axis);
    public Vector2 MousePosition => _interactionContext.MousePosition;
    public Vector2 MouseDelta => _interactionContext.MouseDelta;
    public float MouseWheelDelta => _interactionContext.MouseWheelDelta;
    
    public bool IsButtonPressed(GamepadButton button) => _interactionContext.IsButtonPressed(button);
    public bool IsButtonDown(GamepadButton button) => _interactionContext.IsButtonDown(button);
    public bool IsButtonReleased(GamepadButton button) => _interactionContext.IsButtonReleased(button);
    public bool IsAnyButtonDown() => _interactionContext.IsAnyGamepadButtonDown();
    public void GetButtonsDown(IList<GamepadButton> buttons) => _interactionContext.ButtonsDown(buttons);
    public float GetAxis(GamepadAxis axis) => _interactionContext.Axis(axis);

    #endregion Queries

    #region Manipulation

    public bool AddBinding(ShortcutBinding binding)
    {
        if (_interactionContext.Bindings is null)
        {
            return false;
        }
        
        _interactionContext.Bindings.Add(binding);
        return true;
    }

    public bool AddBinding(string name, Shortcut shortcut)
    {
        var binding = new ShortcutBinding(name, shortcut);
        return AddBinding(binding);
    }

    #endregion Manipulation

    #region Private

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

        if (_interactionContext.Mouse is not null)
        {
            _interactionContext.Mouse.Cursor = cursor;
        }
    }

    #endregion Private

    #region Debug

    private void AddDebugBindings()
    {
        _receiver.Name = "input_debug";
            
        if (_interactionContext.Bindings is null)
        {
            return;
        }
        
        AddBinding(new ShortcutBinding("imgui_metrics", new KeyboardShortcut(Key.F11, Modifiers.Shift)));
        AddBinding(new ShortcutBinding("dbg_draw", new KeyboardShortcut(Key.F12, Modifiers.Shift)));
        
        _receiver.Bind("imgui_metrics", InputState.Released, ToggleImGuiMetrics);
        _receiver.Bind("dbg_draw", InputState.Released, ToggleDebugDraw);
        _receiver.Push();
    }
    
    private void ToggleImGuiMetrics(ref ActionEvent evt)
    {
        evt.Consumed = true;

        _debugDrawImGuiMetrics = !_debugDrawImGuiMetrics;
    }

    private void ToggleDebugDraw(ref ActionEvent evt)
    {
        evt.Consumed = true;
        
        if (_interactionContext.IsDebugging)
        {
            _interactionContext.DisableDebug();
        }
        else
        {
            _interactionContext.EnableDebug();
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