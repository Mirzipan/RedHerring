namespace RedHerring.Game;

public abstract class ASessionContext
{
    public virtual string Name { get; }
    public IList<Type> Components { get; } // TODO: replace with definition
}