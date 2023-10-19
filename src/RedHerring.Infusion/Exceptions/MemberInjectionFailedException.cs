namespace RedHerring.Infusion.Exceptions;

public class MemberInjectionFailedException : Exception
{
    public MemberInjectionFailedException(Type type, Type member, Exception exception) : base($"Failed to inject `{member}` in `{type}`.", exception)
    {
    }
}