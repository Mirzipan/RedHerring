namespace RedHerring.Clues;

public abstract class DefinitionException : Exception
{
    protected DefinitionException() : base()
    {
    }

    protected DefinitionException(string message) : base(message)
    {
    }

    protected DefinitionException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}