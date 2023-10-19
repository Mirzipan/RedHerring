namespace RedHerring.Infusion.Exceptions;

public class MethodInjectionFailedException : Exception
{
    public MethodInjectionFailedException(Type type, string methodName, Exception exception) : base($"Failed to inject method `{methodName}` in `{type}`.", exception)
    {
    }
}