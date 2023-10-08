using System.Numerics;
using RedHerring.Fingerprint;
using Silk.NET.Input;

namespace RedHerring.Engines.Components;

public class InputComponent : AnEngineComponent
{
    private Input _input = null!;

    #region Lifecycle

    protected override void Init()
    {
        _input = new Input(Context.View);
        //_input.EnableDebug();
    }

    #endregion Lifecycle

    #region Queries

    public bool IsKeyUp(Key key) => _input.IsKeyUp(key);
    public bool IsKeyDown(Key key) => _input.IsKeyDown(key);
    public bool IsButtonUp(MouseButton button) => _input.IsButtonUp(button);
    public bool IsButtonDown(MouseButton button) => _input.IsButtonDown(button);
    public Vector2 MousePosition => _input.MousePosition;
    public bool IsButtonUp(ButtonName button) => _input.IsButtonUp(button);
    public bool IsButtonDown(ButtonName button) => _input.IsButtonDown(button);

    #endregion Queries
}