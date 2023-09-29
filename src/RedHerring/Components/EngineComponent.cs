using RedHerring.Core.Components;

namespace RedHerring.Components;

public class EngineComponent : AComponent<EngineComponentCollection>
{
    public override EngineComponentCollection Container { get; }
}