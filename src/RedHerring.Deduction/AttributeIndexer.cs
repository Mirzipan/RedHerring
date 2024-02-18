namespace RedHerring.Deduction;

public interface AttributeIndexer : MetadataIndexer
{
    void Index(Attribute attribute, Type type);
}