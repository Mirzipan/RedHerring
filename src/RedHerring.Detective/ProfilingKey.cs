namespace RedHerring.Detective;

public class ProfilingKey
{
    public string Name { get; init; }
    public ProfilingKey? Parent { get; init; }
    public List<ProfilingKey> Children { get; init; }
    
    public ProfilingKey(string name)
    {
        Name = name;
        Parent = null;
        Children = new();
    }
    
    public ProfilingKey(string name, ProfilingKey parent)
    {
        Name = name;
        Parent = parent;
        Children = new();
    }
}