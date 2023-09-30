﻿using System.Collections;
using RedHerring.Core.Components;

namespace RedHerring.Games;

public sealed class GameComponentCollection : IComponentContainer, IGameComponentCollection, IDisposable
{
    private readonly Dictionary<Type, GameComponent> _componentIndex = new();
    private readonly List<GameComponent> _components = new();
    private readonly List<IUpdate> _updatables = new();
    private readonly List<IDraw> _drawables = new();
    
    private readonly List<IUpdate> _currentlyUpdatingComponents = new();
    private readonly List<IDraw> _currentlyDrawingComponents = new();

    #region Lifecycle
    
    public GameComponentCollection()
    {
    }

    internal void Update(GameTime gameTime)
    {
        _currentlyUpdatingComponents.AddRange(_updatables);
        
        int count = _currentlyUpdatingComponents.Count;
        for (int i = 0; i < count; i++)
        {
            var drawable = _currentlyUpdatingComponents[i];
            drawable.Update(gameTime);
        }
        
        _currentlyUpdatingComponents.Clear();
    }

    internal void Draw(GameTime gameTime)
    {
        _currentlyDrawingComponents.AddRange(_drawables);
        
        int count = _currentlyDrawingComponents.Count;
        for (int i = 0; i < count; i++)
        {
            var drawable = _currentlyDrawingComponents[i];
            if (drawable.BeginDraw())
            {
                drawable.Draw(gameTime);
                drawable.EndDraw();
            }
        }
        
        _currentlyDrawingComponents.Clear();
    }

    public void Dispose()
    {
        int count = _components.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            var disposable = _components[i] as IDisposable;
            disposable?.Dispose();
        }
    }

    #endregion Lifecycle

    #region Manipulation

    private void Sort()
    {
        _updatables.Sort(CompareUpdatables);
        _drawables.Sort(CompareDrawables);
    }

    private int CompareUpdatables(IUpdate lhs, IUpdate rhs) => lhs.UpdateOrder.CompareTo(rhs.UpdateOrder);
    
    private int CompareDrawables(IDraw lhs, IDraw rhs) => lhs.DrawOrder.CompareTo(rhs.DrawOrder);

    #endregion Manipulation

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