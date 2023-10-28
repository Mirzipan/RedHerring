namespace RedHerring.Deduction;

public interface IIndexAttributes : IIndexMetadata
{
    void Index(Attribute attribute, Type type);
}