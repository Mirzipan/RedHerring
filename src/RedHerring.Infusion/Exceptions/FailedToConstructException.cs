namespace RedHerring.Infusion.Exceptions;

public class FailedToConstructException : Exception
{
    public FailedToConstructException(Type type, Exception e) : base($"Failed to construct object of type `{type}`.",e)
    {
    }
}