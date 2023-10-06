using RedHerring.Core.Components;
using Veldrid;

namespace RedHerring.Render.Components;

public class RendererComponentCollection : IComponentContainer
{
    private List<ARendererComponent> _components;
    private bool _isDirty;

    public bool IsDirty => _isDirty;

    #region Lifecycle

    public RendererComponentCollection()
    {
        _components = new List<ARendererComponent>();
    }

    public void Draw(CommandList commandList)
    {
        if (_components.Count == 0)
        {
            return;
        }

        if (IsDirty)
        {
            Sort();
        }
        
        for (int i = 0; i < _components.Count; i++)
        {
            var component = _components[i];
            component.Draw(commandList);
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

    public TComponent Add<TComponent>(TComponent component) where TComponent : ARendererComponent
    {
        AddInternal(component);
        return component;
    }

    #endregion Manipulation

    #region Internal

    internal void AddInternal<TComponent>(TComponent component) where TComponent : ARendererComponent
    {
        _components.Add(component);
        component.SetContainer(this);
    }

    internal void Sort()
    {
        _components.Sort(CompareComponent);
        _isDirty = false;
    }
    
    private int CompareComponent(ARendererComponent lhs, ARendererComponent rhs) => -lhs.Priority.CompareTo(rhs.Priority);

    #endregion Internal
}