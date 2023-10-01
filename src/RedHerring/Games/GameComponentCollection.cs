using System.Collections;
using RedHerring.Core.Components;

namespace RedHerring.Games;

public sealed class GameComponentCollection : IGameComponentCollection, IDisposable
{
    public Game Game { get; }
    
    private readonly Dictionary<Type, AGameComponent> _componentIndex = new();
    private readonly List<AGameComponent> _components = new();
    private readonly List<IUpdatable> _updatables = new();
    private readonly List<IDrawable> _drawables = new();
    
    private readonly List<IUpdatable> _currentlyUpdatingComponents = new();
    private readonly List<IDrawable> _currentlyDrawingComponents = new();

    #region Lifecycle
    
    public GameComponentCollection(Game game)
    {
        Game = game;
    }

    internal void Update(GameTime gameTime)
    {
        _currentlyUpdatingComponents.AddRange(_updatables);
        
        int count = _currentlyUpdatingComponents.Count;
        for (int i = 0; i < count; i++)
        {
            var component = _currentlyUpdatingComponents[i];
            if (!component.IsEnabled)
            {
                continue;
            }

            component.Update(gameTime);
        }
        
        _currentlyUpdatingComponents.Clear();
    }

    internal void Draw(GameTime gameTime)
    {
        _currentlyDrawingComponents.AddRange(_drawables);
        
        int count = _currentlyDrawingComponents.Count;
        for (int i = 0; i < count; i++)
        {
            var component = _currentlyDrawingComponents[i];
            if (!component.IsVisible)
            {
                continue;
            }
            
            if (component.BeginDraw())
            {
                component.Draw(gameTime);
                component.EndDraw();
            }
        }
        
        _currentlyDrawingComponents.Clear();
    }

    public void Dispose()
    {
        int count = _components.Count;
        _components.Clear();
    }

    #endregion Lifecycle

    #region Manipulation

    private void Sort()
    {
        _updatables.Sort(CompareUpdatables);
        _drawables.Sort(CompareDrawables);
    }

    private int CompareUpdatables(IUpdatable lhs, IUpdatable rhs) => lhs.UpdateOrder.CompareTo(rhs.UpdateOrder);
    
    private int CompareDrawables(IDrawable lhs, IDrawable rhs) => lhs.DrawOrder.CompareTo(rhs.DrawOrder);

    #endregion Manipulation

    #region Queries
    
    IComponent? IComponentContainer.Get(Type type)
    {
        return _componentIndex.TryGetValue(type, out var value) ? value : null;
    }

    public T? Get<T>() where T : AGameComponent
    {
        return _componentIndex.TryGetValue(typeof(T), out var value) ? (T)value : null;
    }

    public bool TryGet<T>(out T? component) where T : AGameComponent
    {
        if (_componentIndex.TryGetValue(typeof(T), out var value))
        {
            component = (T)value;
            return true;
        }

        component = null;
        return false;
    }

    public bool TryGet(Type type, out AGameComponent? component)
    {
        return _componentIndex.TryGetValue(type, out component);
    }
    
    #endregion Queries

    #region IEnumerable

    public IEnumerator<AGameComponent> GetEnumerator()
    {
        return _components.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _components.GetEnumerator();
    }
    
    #endregion IEnumerable
}