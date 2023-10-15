﻿namespace RedHerring.Alexandria.Disposables;

public static class DisposableExtensions
{
    /// <summary>
    /// Adds this <see cref="IDisposable"/> to the disposer.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="container">Object with disposer</param>
    /// <returns></returns>
    public static IDisposable DisposeWith(this IDisposable @this, IDisposerContainer container)
    {
        container.Disposer.Add(@this);
        return @this;
    }

    public static IDisposable? TryDisposeWith(this object @this, IDisposerContainer container)
    {
        if (@this is IDisposable disposable)
        {
            container.Disposer.Add(disposable);
            return disposable;
        }

        return null;
    }
}