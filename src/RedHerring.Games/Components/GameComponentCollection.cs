using System.Collections;
using RedHerring.Core;
using RedHerring.Core.Components;

namespace RedHerring.Games.Components;

public sealed class GameComponentCollection : IComponentContainer, IGameComponentCollection
{
    private readonly Dictionary<Type, GameComponent> _componentIndex = new();
    private readonly List<GameComponent> _components = new();
    private readonly List<IUpdatable> _updatables = new();
    private readonly List<IDrawable> _drawables = new();
    
    private readonly List<IUpdatable> _currentlyUpdatingComponents = new();
    private readonly List<IDrawable> _currentlyDrawingComponents = new();

    #region Lifecycle
    
    public GameComponentCollection()
    {
    }

    internal void Update(GameTime gameTime)
    {
        _currentlyUpdatingComponents.AddRange(_updatables);
        
        int count = _updatables.Count;
        for (int i = 0; i < count; i++)
        {
            var drawable = _updatables[i];
            drawable.Update(gameTime);
        }
        
        _currentlyUpdatingComponents.Clear();
    }

    internal void Draw(GameTime gameTime)
    {
        _currentlyDrawingComponents.AddRange(_drawables);
        
        int count = _drawables.Count;
        for (int i = 0; i < count; i++)
        {
            var drawable = _drawables[i];
            if (drawable.BeginDraw())
            {
                drawable.Draw(gameTime);
                drawable.EndDraw();
            }
        }
        
        _currentlyDrawingComponents.Clear();
    }

    #endregion Lifecycle

    #region Queries
    
    AComponent? IComponentContainer.Get(Type type)
    {
        return _componentIndex.TryGetValue(type, out var value) ? value : null;
    }

    public T? Get<T>() where T : GameComponent
    {
        return _componentIndex.TryGetValue(typeof(T), out var value) ? (T)value : null;
    }

    public bool TryGet<T>(out T? component) where T : GameComponent
    {
        if (_componentIndex.TryGetValue(typeof(T), out var value))
        {
            component = (T)value;
            return true;
        }

        component = null;
        return false;
    }

    public bool TryGet(Type type, out GameComponent? component)
    {
        return _componentIndex.TryGetValue(type, out component);
    }
    
    #endregion Queries

    #region IEnumerable

    IEnumerator<GameComponent> IEnumerable<GameComponent>.GetEnumerator()
    {
        return _components.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _components.GetEnumerator();
    }
    
    #endregion IEnumerable
}