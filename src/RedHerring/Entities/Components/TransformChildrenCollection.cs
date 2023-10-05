namespace RedHerring.Entities.Components;

public class TransformChildrenCollection : IDisposable
{
    private List<TransformComponent> _components = new();

    private readonly TransformComponent _transform;
    private Entity? Entity => _transform.Entity;

    public TransformComponent this[int index]
    {
        get => _components[index];
    }

    #region Lifecycle

    public TransformChildrenCollection(TransformComponent transformParam)
    {
        _transform = transformParam;
    }

    public void Dispose()
    {
        _components.Clear();
    }

    #endregion Lifecycle

    #region Manipulation

    protected void Set(int index, TransformComponent item)
    {
        OnTransformRemoved(this[index]);

        _components[index] = item;

        OnTransformAdded(this[index]);
    }

    protected void Insert(int index, TransformComponent item)
    {
        _components.Insert(index, item);
        OnTransformAdded(item);
    }

    protected void Remove(int index)
    {
        OnTransformRemoved(this[index]);
        _components.RemoveAt(index);
    }

    protected void Clear()
    {
        int count = _components.Count;
        for (int i = count - 1; i >= 0; --i)
        {
            OnTransformRemoved(this[i]);
        }

        _components.Clear();
    }

    #endregion Manipulation

    #region Bindings

    private void OnTransformAdded(TransformComponent item)
    {
        if (item.Parent != null)
        {
            throw new InvalidOperationException("This TransformComponent already has a Parent, detach it first.");
        }

        item.Parent = _transform;
    }

    private void OnTransformRemoved(TransformComponent item)
    {
        if (item.Parent != _transform)
        {
            throw new InvalidOperationException("This TransformComponent's parent is not the expected value.");
        }

        item.Parent = null;
    }

    #endregion Bindings
}