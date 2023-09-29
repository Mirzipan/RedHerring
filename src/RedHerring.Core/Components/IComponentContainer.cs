namespace RedHerring.Core.Components;

public interface IComponentContainer
{
    AComponent? Get(Type type);
}