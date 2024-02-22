using RedHerring.Core;
using RedHerring.Fingerprint;
using RedHerring.Infusion.Attributes;
using RedHerring.Studio.Constants;

namespace RedHerring.Studio.Systems;

internal sealed class Configuration : EngineSystem
{
    [Infuse]
    private InteractionContext _interactionContext = null!;
    
    protected override void Init()
    {
        Input();
    }

    private void Input()
    {
        _interactionContext.AddKeyboardBinding(InputAction.Undo, Key.U);
        _interactionContext.AddKeyboardBinding(InputAction.Redo, Key.Z);
        
        _interactionContext.AddKeyboardBinding(InputAction.MoveLeft, Key.A);
        _interactionContext.AddKeyboardBinding(InputAction.MoveRight, Key.D);
        _interactionContext.AddKeyboardBinding(InputAction.MoveUp, Key.Space);
        _interactionContext.AddKeyboardBinding(InputAction.MoveDown, Key.C);
        _interactionContext.AddKeyboardBinding(InputAction.MoveForward, Key.W);
        _interactionContext.AddKeyboardBinding(InputAction.MoveBackward, Key.S);

        _interactionContext.AddMouseBinding(InputAction.MoveSpeedIncrease, MouseAxis.WheelUp);
        _interactionContext.AddMouseBinding(InputAction.MoveSpeedDecrease, MouseAxis.WheelDown);
        
        _interactionContext.AddKeyboardBinding(InputAction.ReloadShaders, Key.F10, Modifiers.Shift);
    }
}