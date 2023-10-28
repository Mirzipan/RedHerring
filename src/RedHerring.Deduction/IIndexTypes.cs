namespace RedHerring.Deduction;

public interface IIndexTypes : IIndexMetadata
{
    void Index(Type type);
}