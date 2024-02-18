namespace RedHerring.Deduction;

public interface TypeIndexer : MetadataIndexer
{
    void Index(Type type);
}