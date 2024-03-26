using System.Runtime.CompilerServices;

namespace RedHerring.Fingerprint.Layers;

internal class Processor
{
    private record struct TriggeredInput(InputState State, float Value)
    {
        public bool Consumed = false;
    }

    private record struct UnconsumedInput(string Name, int Index);

    private readonly List<UnconsumedInput> _unconsumed = new();
    private readonly List<TriggeredInput> _queue = new();

    private readonly Dictionary<Shortcut, InputState> _triggeredShortcuts = new();

    #region Lifecycle
    
    public void NextFrame(InteractionContext context)
    {
        if (GatherInput(context))
        {
            ConsumeInput(context);
        }
    }

    #endregion Lifecycle

    #region Private

    private bool GatherInput(InteractionContext context)
    {
        _triggeredShortcuts.Clear();

        var bindings = context.Bindings;
        if (bindings is null || bindings.Count == 0)
        {
            return false;
        }

        for (int i = 0; i < bindings.Count; i++)
        {
            var binding = bindings[i];
            var state = context.State(binding.Shortcut);
            if (state == InputState.Up)
            {
                continue;
            }

            _triggeredShortcuts[binding.Shortcut] = state;
        }

        foreach (var pair in _triggeredShortcuts)
        {
            var actions = bindings.ActionsForShortcut(pair.Key);
            QueueInput(actions!, pair.Value, context.AnalogValue(pair.Key));
            foreach (string action in actions!)
            {
                context.OnActionChanged(action, pair.Value);
            }
        }

        return _triggeredShortcuts.Count > 0;
    }

    private void QueueInput(IReadOnlyCollection<string> actions, InputState state, float value)
    {
        int index = _queue.Count;
        _queue.Add(new TriggeredInput(state, value));
        foreach (string entry in actions)
        {
            _unconsumed.Add(new UnconsumedInput(entry, index));
        }
    }

    private void ConsumeInput(InteractionContext context)
    {
        var layers = context.Layers;
        if (layers.Stack.Count == 0)
        {
            goto clear;
        }

        for (int i = layers.Stack.Count - 1; i >= 0; i--)
        {
            var receiver = layers.Stack[i];
            if (receiver is null)
            {
                continue;
            }

            ConsumeInput(receiver);
            if (receiver.ConsumesAllInput)
            {
                goto clear;
            }
        }

        clear:
        _unconsumed.Clear();
        _queue.Clear();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ConsumeInput(InputLayer inputLayer)
    {
        foreach (var entry in _unconsumed)
        {
            var shortcut = _queue[entry.Index];
            if (shortcut.Consumed)
            {
                continue;
            }
            
            if (!inputLayer.TryGetBinding(entry.Name, out var binding) || (binding.State & shortcut.State) == 0)
            {
                continue;
            }

            var actionEvent = new ActionEvent(entry.Name, shortcut.State, shortcut.Value);
            binding.Handler(ref actionEvent);
            if (actionEvent.Consumed)
            {
                shortcut.Consumed = true;
                _queue[entry.Index] = shortcut;
            }
        }
    }

    #endregion Private
}