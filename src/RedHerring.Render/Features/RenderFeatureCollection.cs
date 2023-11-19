using RedHerring.Alexandria.Components;
using RedHerring.Render.Passes;
using Silk.NET.Maths;
using Veldrid;

namespace RedHerring.Render.Features;

public class RenderFeatureCollection : IComponentContainer, IDisposable
{
    private List<RenderFeature> _features = new();
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
            feature.RaiseInit(device, commandList);
        }
    }

    public void Unload(GraphicsDevice device, CommandList commandList)
    {
        if (_features.Count == 0)
        {
            return;
        }
        
        for (int i = 0; i < _features.Count; i++)
        {
            var feature = _features[i];
            feature.RaiseUnload(device, commandList);
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

    public void Render(GraphicsDevice device, CommandList commandList, RenderEnvironment environment, RenderPass pass)
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
            feature.Render(device, commandList, environment, pass);
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

    internal TComponent Add<TComponent>(TComponent component) where TComponent : RenderFeature
    {
        AddInternal(component);
        return component;
    }

    #endregion Manipulation

    #region Internal

    internal void AddInternal<TComponent>(TComponent component) where TComponent : RenderFeature
    {
        _features.Add(component);
        component.SetContainer(this);
    }

    internal void Sort()
    {
        _features.Sort(CompareComponent);
        _isDirty = false;
    }
    
    private int CompareComponent(RenderFeature lhs, RenderFeature rhs) => -lhs.Priority.CompareTo(rhs.Priority);

    #endregion Internal
}