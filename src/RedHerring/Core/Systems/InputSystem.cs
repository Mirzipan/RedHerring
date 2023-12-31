﻿using System.Numerics;
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
    private Input _input = null!;
    [Infuse]
    private InputReceiver _receiver = null!;

    public bool IsEnabled => true;
    public int UpdateOrder => -1_000_000;

    public Input Input => _input;
    public KeyboardState? Keyboard => _input.Keyboard;
    public MouseState? Mouse => _input.Mouse;

    private bool _debugDrawImGuiMetrics = false;

    #region Lifecycle

    protected override void Init()
    {
        AddDebugBindings();
    }
    public void Update(GameTime gameTime)
    {
        UpdateCursor();
        ImGuiProxy.Update(gameTime, _input);
        
        _input.Tick();
    }

    #endregion Lifecycle

    #region Queries

    public bool IsKeyPressed(Key key) => _input.IsKeyPressed(key);
    public bool IsKeyDown(Key key) => _input.IsKeyDown(key);
    public bool IsKeyReleased(Key key) => _input.IsKeyReleased(key);
    public bool IsAnyKeyDown() => _input.IsAnyKeyDown();
    public void GetKeysDown(IList<Key> keys) => _input.KeysDown(keys);
    
    public bool IsButtonPressed(MouseButton button) => _input.IsButtonPressed(button);
    public bool IsButtonDown(MouseButton button) => _input.IsButtonDown(button);
    public bool IsButtonReleased(MouseButton button) => _input.IsButtonReleased(button);
    public bool IsAnyMouseButtonDown() => _input.IsAnyMouseButtonDown();
    public void GetButtonsDown(IList<MouseButton> buttons) => _input.ButtonsDown(buttons);
    public bool IsMouseMoved(MouseAxis axis) => _input.IsMouseMoved(axis);
    public float GetAxis(MouseAxis axis) => _input.Axis(axis);
    public Vector2 MousePosition => _input.MousePosition;
    public Vector2 MouseDelta => _input.MouseDelta;
    public float MouseWheelDelta => _input.MouseWheelDelta;
    
    public bool IsButtonPressed(GamepadButton button) => _input.IsButtonPressed(button);
    public bool IsButtonDown(GamepadButton button) => _input.IsButtonDown(button);
    public bool IsButtonReleased(GamepadButton button) => _input.IsButtonReleased(button);
    public bool IsAnyButtonDown() => _input.IsAnyGamepadButtonDown();
    public void GetButtonsDown(IList<GamepadButton> buttons) => _input.ButtonsDown(buttons);
    public float GetAxis(GamepadAxis axis) => _input.Axis(axis);

    #endregion Queries

    #region Manipulation

    public bool AddBinding(ShortcutBinding binding)
    {
        if (_input.Bindings is null)
        {
            return false;
        }
        
        _input.Bindings.Add(binding);
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

        if (_input.Mouse is not null)
        {
            _input.Mouse.Cursor = cursor;
        }
    }

    #endregion Private

    #region Debug

    private void AddDebugBindings()
    {
        _receiver.Name = "input_debug";
            
        if (_input.Bindings is null)
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
        
        if (_input.IsDebugging)
        {
            _input.DisableDebug();
        }
        else
        {
            _input.EnableDebug();
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