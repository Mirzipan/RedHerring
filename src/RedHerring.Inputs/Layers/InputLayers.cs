using RedHerring.Alexandria.Identifiers;

namespace RedHerring.Inputs.Layers;

public class InputLayers
{
    private readonly List<InputLayer?> _stack = new();
    private readonly Dictionary<StringId, int> _fixedLayers = new();

    public IReadOnlyList<InputLayer?> Stack => _stack;
    
    public event LayerPushed? Pushed;
    public event LayerPopped? Popped;

    #region Queries

    public InputLayer? Peek() => _stack.Count != 0 ? _stack[^1] : null;

    #endregion Queries

    #region Manipulation

    internal void SetLayers(IList<StringId> layers)
    {
        PopAll();
        _fixedLayers.Clear();

        for (int i = 0; i < layers.Count; i++)
        {
            _fixedLayers.Add(layers[i], i);
        }
    }
    
    public void Push(InputLayer inputLayer)
    {
        if (inputLayer.IsActive)
        {
            return;
        }

        if (!inputLayer.Layer.IsEmpty)
        {
            SetAt(inputLayer.Layer, inputLayer);
        }
        else
        {
            PadBelow(_fixedLayers.Count);
            Add(inputLayer);
        }
    }

    public void Pop(InputLayer inputLayer)
    {
        int index = _stack.IndexOf(inputLayer);
        if (index < 0)
        {
            return;
        }

        if (index < _fixedLayers.Count)
        {
            ReplaceByNull(index);
        }
        else
        {
            RemoveAt(index);
        }
    }

    public void PopAll()
    {
        while (_stack.Count > 0)
        {
            ReplaceByNull(_stack.Count - 1);
        }
    }

    #endregion Manipulation

    #region Internal

    private void Add(InputLayer inputLayer)
    {
        _stack.Add(inputLayer);
        HandleAdded(inputLayer);
    }

    private void SetAt(int index, InputLayer inputLayer)
    {
        _stack[index] = inputLayer;
        HandleAdded(inputLayer);
    }

    private void SetAt(StringId layer, InputLayer inputLayer)
    {
        if (!_fixedLayers.TryGetValue(layer, out int index))
        {
            // Trying to push to an undefined layer
            return;
        }

        if (_stack.Count > index && inputLayer == _stack[index])
        {
            // Trying to push layer to a position it already occupies
            return;
        }

        if (_stack.Count > index)
        {
            ReplaceByNull(index);
            SetAt(index, inputLayer);
        }
        else
        {
            PadBelow(index);
            Add(inputLayer);
        }
    }

    private void PadBelow(int index)
    {
        while (_stack.Count < index)
        {
            _stack.Add(null);
        }
    }

    private void ReplaceByNull(int index)
    {
        var receiver = _stack[index];
        if (receiver is not null)
        {
            _stack[index] = null;
            HandleRemoved(receiver);
        }
    }

    private void RemoveAt(int index)
    {
        var receiver = _stack[index];
        _stack.RemoveAt(index);

        if (receiver is not null)
        {
            HandleRemoved(receiver);
        }
    }

    private void HandleAdded(InputLayer inputLayer)
    {
        inputLayer.IsActive = true;
        inputLayer.RaisePushed();
        RaisePushed(inputLayer);
    }

    private void HandleRemoved(InputLayer inputLayer)
    {
        inputLayer.IsActive = false;
        inputLayer.RaisePopped();
        RaisePopped(inputLayer);
    }

    private void RaisePushed(InputLayer inputLayer) => Pushed?.Invoke(inputLayer);
    
    private void RaisePopped(InputLayer inputLayer) => Popped?.Invoke(inputLayer);

    #endregion Internal
}