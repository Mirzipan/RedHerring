using RedHerring.Alexandria;
using RedHerring.Core;
using RedHerring.Core.Systems;
using RedHerring.Fingerprint;
using RedHerring.Fingerprint.Events;
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

        CreateShortcuts();
        SubscribeToInput();
    }

    protected override ValueTask<int> Load()
    {
        _feature = new ImGuiRenderFeature();
        _graphicsSystem.RegisterFeature(_feature);
        return ValueTask.FromResult(0);
    }

    protected override ValueTask<int> Unload()
    {
        UnsubscribeFromInput();
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

    #region Private

    private void SubscribeToInput()
    {
        _inputSystem.Input.KeyEvent += OnKeyEvent;
        _inputSystem.Input.MouseButtonEvent += OnMouseButtonEvent;
        _inputSystem.Input.MouseAxisEvent += OnMouseAxisEvent;
        _inputSystem.Input.GamepadButtonEvent += OnGamepadButtonEvent;
        _inputSystem.Input.GamepadAxisEvent += OnGamepadAxisEvent;
    }

    private void UnsubscribeFromInput()
    {
        _inputSystem.Input.KeyEvent -= OnKeyEvent;
        _inputSystem.Input.MouseButtonEvent -= OnMouseButtonEvent;
        _inputSystem.Input.MouseAxisEvent -= OnMouseAxisEvent;
        _inputSystem.Input.GamepadButtonEvent -= OnGamepadButtonEvent;
        _inputSystem.Input.GamepadAxisEvent -= OnGamepadAxisEvent;
    }
    
    private void CreateShortcuts()
    {
        var shortcut = new CompositeShortcut();
        shortcut.Add(new KeyboardShortcut(Key.ShiftLeft));
        shortcut.Add(new KeyboardShortcut(Key.F11));
        _inputSystem.AddBinding("imgui_metrics", shortcut);

        _receiver.Name = "ImGui";
        _receiver.Bind("imgui_metrics", InputState.Released, ToggleFontDebug);
        _receiver.Push();
    }

    #endregion Private

    #region Bindings
    
    private void ToggleFontDebug(ref ActionEvent evt)
    {
        evt.Consumed = true;

        _debugDraw = !_debugDraw;
    }

    private void OnKeyEvent(KeyChanged evt)
    {
        Gui.GetIO().AddKeyEvent(Convert.ToImGuiKey(evt.Key), evt.IsDown);
    }

    private void OnMouseButtonEvent(MouseButtonChanged evt)
    {
        Gui.GetIO().AddMouseButtonEvent((int)evt.Button,  evt.IsDown);
    }

    private void OnMouseAxisEvent(MouseAxisMoved evt)
    {
        switch (evt.Axis)
        {
            case MouseAxis.None:
                break;
            case MouseAxis.Horizontal:
                break;
            case MouseAxis.Vertical:
                break;
            case MouseAxis.HorizontalDelta:
                break;
            case MouseAxis.VerticalDelta:
                break;
            case MouseAxis.Wheel:
                break;
            case MouseAxis.WheelUp:
                break;
            case MouseAxis.WheelDown:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnGamepadButtonEvent(GamepadButtonChanged evt)
    {
        Gui.GetIO().AddKeyEvent(Convert.ToImGuiKey(evt.Button),  evt.IsDown);
    }

    private void OnGamepadAxisEvent(GamepadAxisMoved evt)
    {
        switch (evt.Axis)
        {
            case GamepadAxis.None:
                break;
            case GamepadAxis.LeftX:
                break;
            case GamepadAxis.LeftY:
                break;
            case GamepadAxis.RightX:
                break;
            case GamepadAxis.RightY:
                break;
            case GamepadAxis.TriggerLeft:
                break;
            case GamepadAxis.TriggerRight:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #endregion Bindings
}