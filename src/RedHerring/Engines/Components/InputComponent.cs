using System.Numerics;
using RedHerring.Alexandria;
using RedHerring.Fingerprint;
using Silk.NET.Input;

namespace RedHerring.Engines.Components;

public class InputComponent : AnEngineComponent, IUpdatable
{
    private Input _input = null!;

    public bool IsEnabled => true;
    public int UpdateOrder => -1_000_000;

    #region Lifecycle

    protected override void Init()
    {
        _input = new Input(Context.View);
        _input.EnableDebug();
    }
    public void Update(GameTime gameTime)
    {
        _input.Tick();
    }

    #endregion Lifecycle

    #region Queries

    public bool IsKeyUp(Key key) => _input.IsKeyUp(key);
    public bool IsKeyPressed(Key key) => _input.IsKeyPressed(key);
    public bool IsKeyDown(Key key) => _input.IsKeyDown(key);
    public bool IsKeyReleased(Key key) => _input.IsKeyReleased(key);
    
    public bool IsButtonUp(MouseButton button) => _input.IsButtonUp(button);
    public bool IsButtonPressed(MouseButton button) => _input.IsButtonPressed(button);
    public bool IsButtonDown(MouseButton button) => _input.IsButtonDown(button);
    public bool IsButtonReleased(MouseButton button) => _input.IsButtonReleased(button);
    public bool IsMouseMoved(MouseAxis axis) => _input.IsMouseMoved(axis);
    public Vector2 MousePosition => _input.MousePosition;
    
    public bool IsButtonUp(ButtonName button) => _input.IsButtonUp(button);
    public bool IsButtonDown(ButtonName button) => _input.IsButtonDown(button);

    #endregion Queries
}