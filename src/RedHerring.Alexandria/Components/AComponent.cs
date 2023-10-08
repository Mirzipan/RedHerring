namespace RedHerring.Alexandria.Components;

public abstract class AComponent : IComponent
{
    public abstract IComponentContainer Container { get; }

    internal AComponent()
    {
    }
}