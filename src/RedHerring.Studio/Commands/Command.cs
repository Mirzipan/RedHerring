namespace RedHerring.Studio.Commands;

public abstract class Command
{
    public abstract void Do();
    public abstract void Undo();
}