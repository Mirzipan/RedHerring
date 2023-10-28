using System.Reflection;

namespace RedHerring.Deduction;

public sealed class AssemblyCollection : IDisposable
{
    private readonly HashSet<Assembly> _knownAssemblies = new();

    public IReadOnlyCollection<Assembly> KnownAssemblies => _knownAssemblies;

    #region Lifecycle

    public void Add(IEnumerable<Assembly> assemblies)
    {
        foreach (Assembly entry in assemblies)
        {
            Add(entry);
        }
    }

    public void Add(params Assembly[] assemblies)
    {
        Add((IEnumerable<Assembly>)assemblies);
    }

    public void Add(Assembly assembly)
    {
        if (_knownAssemblies.Contains(assembly))
        {
            return;
        }

        _knownAssemblies.Add(assembly);
    }

    public void Dispose()
    {
        _knownAssemblies.Clear();
    }

    #endregion Lifecycle

    #region Queries

    public IEnumerable<Type> GetAllTypes()
    {
        foreach (var assembly in _knownAssemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                yield return type;
            }
        }
    }

    #endregion Queries
}