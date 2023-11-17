namespace RedHerring.Alexandria.Components;

public abstract class Component<TContainer> : AComponent where TContainer : class, IComponentContainer
{
    public abstract override TContainer? Container { get; }
}