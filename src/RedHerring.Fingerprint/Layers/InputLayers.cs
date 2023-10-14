using RedHerring.Alexandria.Identifiers;

namespace RedHerring.Fingerprint.Layers;

public class InputLayers
{
    private readonly List<InputReceiver?> _stack = new();
    private readonly Dictionary<OctoByte, int> _fixedLayers = new();

    public IReadOnlyList<InputReceiver?> Stack => _stack;
    
    public event LayerPushed? Pushed;
    public event LayerPopped? Popped;

    #region Queries

    public InputReceiver? Peek() => _stack.Count != 0 ? _stack[^1] : null;

    #endregion Queries

    #region Manipulation

    internal void SetLayers(IList<OctoByte> layers)
    {
        PopAll();
        _fixedLayers.Clear();

        for (int i = 0; i < layers.Count; i++)
        {
            _fixedLayers.Add(layers[i], i);
        }
    }
    
    public void Push(InputReceiver receiver)
    {
        if (receiver.IsActive)
        {
            return;
        }

        if (receiver.Layer.Value != 0)
        {
            SetAt(receiver.Layer, receiver);
        }
        else
        {
            PadBelow(_fixedLayers.Count);
            Add(receiver);
        }
    }

    public void Pop(InputReceiver receiver)
    {
        int index = _stack.IndexOf(receiver);
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

    private void Add(InputReceiver receiver)
    {
        _stack.Add(receiver);
        HandleAdded(receiver);
    }

    private void SetAt(int index, InputReceiver receiver)
    {
        _stack[index] = receiver;
        HandleAdded(receiver);
    }

    private void SetAt(OctoByte layer, InputReceiver receiver)
    {
        if (!_fixedLayers.TryGetValue(layer, out int index))
        {
            // Trying to push to an undefined layer
            return;
        }

        if (_stack.Count > index && receiver == _stack[index])
        {
            // Trying to push layer to a position it already occupies
            return;
        }

        if (_stack.Count > index)
        {
            ReplaceByNull(index);
            SetAt(index, receiver);
        }
        else
        {
            PadBelow(index);
            Add(receiver);
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

    private void HandleAdded(InputReceiver receiver)
    {
        receiver.IsActive = true;
        receiver.RaisePushed();
        RaisePushed(receiver);
    }

    private void HandleRemoved(InputReceiver receiver)
    {
        receiver.IsActive = false;
        receiver.RaisePopped();
        RaisePopped(receiver);
    }

    private void RaisePushed(InputReceiver receiver) => Pushed?.Invoke(receiver);
    
    private void RaisePopped(InputReceiver receiver) => Popped?.Invoke(receiver);

    #endregion Internal
}