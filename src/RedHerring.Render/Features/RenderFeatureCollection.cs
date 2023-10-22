using RedHerring.Alexandria.Components;
using RedHerring.Render.Passes;
using Silk.NET.Maths;
using Veldrid;

namespace RedHerring.Render.Features;

public class RenderFeatureCollection : IComponentContainer, IDisposable
{
    private List<ARenderFeature> _features = new();
    private bool _isDirty;

    public bool IsDirty => _isDirty;

    #region Lifecycle

    public void Init(GraphicsDevice device, CommandList commandList)
    {
        if (_features.Count == 0)
        {
            return;
        }
        
        for (int i = 0; i < _features.Count; i++)
        {
            var feature = _features[i];
            feature.Init(device, commandList);
        }
    }

    public void Update(GraphicsDevice device, CommandList commandList)
    {
        if (_features.Count == 0)
        {
            return;
        }

        if (IsDirty)
        {
            Sort();
        }
        
        for (int i = 0; i < _features.Count; i++)
        {
            var feature = _features[i];
            feature.Update(device, commandList);
        }
    }

    public void Render(GraphicsDevice device, CommandList commandList, RenderPass pass)
    {
        if (_features.Count == 0)
        {
            return;
        }

        if (IsDirty)
        {
            Sort();
        }
        
        for (int i = 0; i < _features.Count; i++)
        {
            var feature = _features[i];
            feature.Render(device, commandList, pass);
        }
    }

    public void Resize(Vector2D<int> size)
    {
        foreach (var feature in _features)
        {
            feature.Resize(size);
        }
    }

    public void Dispose()
    {
        if (_features.Count == 0)
        {
            return;
        }
        
        for (int i = 0; i < _features.Count; i++)
        {
            var feature = _features[i];
            feature.Dispose();
        }
    }

    #endregion Lifecycle

    #region Queries

    public IComponent? Get(Type type)
    {
        return null;
    }

    #endregion Queries

    #region Manipulation

    public TComponent Add<TComponent>(TComponent component) where TComponent : ARenderFeature
    {
        AddInternal(component);
        return component;
    }

    #endregion Manipulation

    #region Internal

    internal void AddInternal<TComponent>(TComponent component) where TComponent : ARenderFeature
    {
        _features.Add(component);
        component.SetContainer(this);
    }

    internal void Sort()
    {
        _features.Sort(CompareComponent);
        _isDirty = false;
    }
    
    private int CompareComponent(ARenderFeature lhs, ARenderFeature rhs) => -lhs.Priority.CompareTo(rhs.Priority);

    #endregion Internal
}