namespace RedHerring.Alexandria.Components;

public interface IComponentContainer
{
    IComponent? Get(Type type);
}