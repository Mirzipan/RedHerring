using RedHerring.Alexandria.Disposables;

namespace RedHerring.Deduction;

public static class Findings
{
    private static FindingsContext? _context;

    #region Lifecycle
    
    public static FindingsContext CreateContext(AssemblyCollection container)
    {
        var previous = CurrentContext();
        var context = new FindingsContext(container);
        CurrentContext(previous ?? context);
        
        return context;
    }

    public static void DestroyContext(FindingsContext? context = null)
    {
        var previous = CurrentContext();
        if (context is null)
        {
            context = previous;
        }

        CurrentContext(context != previous ? previous : null);
        context.TryDispose();
    }

    public static FindingsContext? CurrentContext() => _context;

    public static void CurrentContext(FindingsContext? context) => _context = context;

    #endregion Lifecycle

    #region Public

    public static void Process() => _context?.Process();
    
    #endregion Public
    
    #region Queries

    public static T? IndexerByType<T>() where T : MetadataIndexer
    {
        return _context is not null ? _context.IndexerByType<T>() : default;
    }

    public static MetadataIndexer? IndexerByType(Type type)
    {
        return _context?.IndexerByType(type);
    }

    #endregion Queries
}