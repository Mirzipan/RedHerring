using RedHerring.Core;
using RedHerring.Fingerprint;
using RedHerring.Infusion.Attributes;
using RedHerring.Studio.Constants;

namespace RedHerring.Studio.Systems;

internal sealed class Configuration : EngineSystem
{
    [Infuse]
    private Input _input = null!;
    
    protected override void Init()
    {
        Input();
    }

    private void Input()
    {
        _input.AddKeyboardBinding(InputAction.Undo, Key.U);
        _input.AddKeyboardBinding(InputAction.Redo, Key.Z);
        
        _input.AddKeyboardBinding(InputAction.MoveLeft, Key.A);
        _input.AddKeyboardBinding(InputAction.MoveRight, Key.D);
        _input.AddKeyboardBinding(InputAction.MoveUp, Key.Space);
        _input.AddKeyboardBinding(InputAction.MoveDown, Key.C);
        _input.AddKeyboardBinding(InputAction.MoveForward, Key.W);
        _input.AddKeyboardBinding(InputAction.MoveBackward, Key.S);

        // TODO: fix mouse axis not working
        _input.AddMouseBinding(InputAction.MoveSpeedIncrease, MouseAxis.WheelUp);
        _input.AddMouseBinding(InputAction.MoveSpeedDecrease, MouseAxis.WheelDown);

    }
}