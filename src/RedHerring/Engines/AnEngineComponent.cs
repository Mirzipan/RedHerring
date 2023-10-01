using RedHerring.Core.Components;

namespace RedHerring.Engines;

public abstract class AnEngineComponent : AComponent<EngineComponentCollection>
{
    public override EngineComponentCollection Container { get; }
}