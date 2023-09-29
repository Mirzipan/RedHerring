namespace RedHerring.Core.Components;

public abstract class AComponent : AnEssence, IComponent
{
    public abstract IComponentContainer Container { get; }

    internal AComponent()
    {
    }
}