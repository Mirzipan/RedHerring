using RedHerring.Fingerprint;

namespace RedHerring.Engines.Components;

public class InputComponent : AnEngineComponent
{
    private Input _input;
    
    protected override void Init()
    {
        _input = new Input();
    }
}