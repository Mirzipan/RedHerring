namespace RedHerring.Exceptions;

public class TypeIsNotAnEngineComponentException : Exception
{
    private string _message;
    public override string Message => _message;

    public TypeIsNotAnEngineComponentException(Type type)
    {
        _message = $"{type} is not an engine component.";
    }

    public TypeIsNotAnEngineComponentException(Type type, Exception? innerException) : base(string.Empty, innerException)
    {
        _message = $"{type} is not an engine component.";
    }
}