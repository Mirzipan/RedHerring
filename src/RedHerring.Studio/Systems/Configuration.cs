using RedHerring.Core;
using RedHerring.Infusion.Attributes;
using RedHerring.Inputs;
using RedHerring.Studio.Constants;

namespace RedHerring.Studio.Systems;

internal sealed class Configuration : EngineSystem
{
    [Infuse]
    private InteractionContext _interactionContext = null!;
    
    protected override void Init()
    {
        InitInput();
    }

    private void InitInput()
    {
        _interactionContext.AddBinding(InputAction.Undo, Input.U);
        _interactionContext.AddBinding(InputAction.Redo, Input.Z);
        
        _interactionContext.AddBinding(InputAction.MoveLeft, Input.A);
        _interactionContext.AddBinding(InputAction.MoveRight, Input.D);
        _interactionContext.AddBinding(InputAction.MoveUp, Input.Space);
        _interactionContext.AddBinding(InputAction.MoveDown, Input.C);
        _interactionContext.AddBinding(InputAction.MoveForward, Input.W);
        _interactionContext.AddBinding(InputAction.MoveBackward, Input.S);

        _interactionContext.AddBinding(InputAction.MoveSpeedIncrease, Input.MouseWheelYPositive);
        _interactionContext.AddBinding(InputAction.MoveSpeedDecrease, Input.MouseWheelYNegative);
        
        _interactionContext.AddBinding(InputAction.ReloadShaders, Input.F10, Modifier.Shift);
    }
}