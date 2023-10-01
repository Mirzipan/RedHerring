namespace RedHerring.Core.Components;

public interface IComponentContainer
{
    IComponent? Get(Type type);
}