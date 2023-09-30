using RedHerring.Core.Components;

namespace RedHerring.Engines;

public abstract class EngineComponent : AComponent<EngineComponentCollection>
{
    public override EngineComponentCollection Container { get; }
}