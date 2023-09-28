using System.Reactive.Disposables;
using RedHerring.Alexandria.Disposables;

namespace RedHerring.Core;

public abstract class AnEssence : IEssence, IDisposerContainer, IDisposable
{
    private string _name;
    private bool _isDisposed;
    private CompositeDisposable _disposer;

    public virtual string Name
    {
        get => _name;
        set => _name = value;
    }
    CompositeDisposable IDisposerContainer.Disposer
    {
        get => _disposer;
        set => _disposer = value;
    }

    public bool IsDisposed => _isDisposed;

    #region Lifecycle

    protected AnEssence(string? name = null)
    {
        _disposer = new CompositeDisposable();
        _name = name ?? GetType().Name;
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _disposer.Dispose();
        _isDisposed = true;
    }

    protected void Destroy()
    {
        OnDestroy();
        
        _disposer.Dispose();
    }

    #endregion Lifecycle

    protected virtual void OnDestroy()
    {
    }
}