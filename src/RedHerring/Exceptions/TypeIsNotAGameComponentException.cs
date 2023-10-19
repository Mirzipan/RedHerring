namespace RedHerring.Exceptions;

public class TypeIsNotAGameComponentException : Exception
{
    private string _message;
    public override string Message => _message;

    public TypeIsNotAGameComponentException(Type type)
    {
        _message = $"{type} is not a game component.";
    }

    public TypeIsNotAGameComponentException(Type type, Exception? innerException) : base(string.Empty, innerException)
    {
        _message = $"{type} is not a game component.";
    }
}