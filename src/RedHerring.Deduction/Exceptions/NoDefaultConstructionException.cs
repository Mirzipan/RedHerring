namespace RedHerring.Deduction.Exceptions;

public class NoDefaultConstructionException : Exception
{
    public NoDefaultConstructionException(Type type) : base($"Type `{type}` has no default constructor.")
    {
    }
}