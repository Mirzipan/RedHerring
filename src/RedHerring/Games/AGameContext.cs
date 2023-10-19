namespace RedHerring.Games;

public abstract class AGameContext
{
    public virtual string Name { get; }
    public IList<Type> Components { get; } // TODO: replace with definition
}