namespace RedHerring.Infusion.Exceptions;

public class FailedToCreateMemberSetterException : Exception
{
    public FailedToCreateMemberSetterException(Type type, string memberName) : base($"Failed to create setter for `{memberName}` in type `{type}`.")
    {
    }
}