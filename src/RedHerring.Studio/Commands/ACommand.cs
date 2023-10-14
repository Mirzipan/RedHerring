namespace RedHerring.Studio.Commands;

public abstract class ACommand
{
    public abstract void Do();
    public abstract void Undo();
}