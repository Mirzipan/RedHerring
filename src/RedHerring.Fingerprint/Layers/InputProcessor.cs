using System.Runtime.CompilerServices;
using RedHerring.Fingerprint.Shortcuts;
using RedHerring.Fingerprint.States;

namespace RedHerring.Fingerprint.Layers;

internal class InputProcessor
{
    private record struct TriggeredInput(InputState State, float Value)
    {
        public bool Consumed = false;
    }

    private record struct UnconsumedInput(string Name, int Index);

    private readonly List<UnconsumedInput> _unconsumed = new();
    private readonly List<TriggeredInput> _queue = new();

    private readonly Dictionary<Shortcut, InputState> _triggeredShortcuts = new();

    private readonly Input _input;
    private readonly InputLayers _layers;
    private readonly ActionsState _actionsState;

    #region Lifecycle

    public InputProcessor(Input input, ActionsState actionsState)
    {
        _input = input;
        _layers = input.Layers;
        _actionsState = actionsState;
    }
    
    public void Tick()
    {
        if (GatherInput())
        {
            ConsumeInput();
        }
    }

    #endregion Lifecycle

    #region Private

    private bool GatherInput()
    {
        _triggeredShortcuts.Clear();

        var bindings = _input.Bindings;
        if (bindings is null || bindings.Count == 0)
        {
            return false;
        }

        for (int i = 0; i < bindings.Count; i++)
        {
            var binding = bindings[i];
            var state = GetShortcutState(binding.Shortcut!);
            if (state == InputState.Up)
            {
                continue;
            }

            _triggeredShortcuts[binding.Shortcut!] = state;
        }

        foreach (var pair in _triggeredShortcuts)
        {
            var actions = bindings.ActionsForShortcut(pair.Key);
            QueueInput(actions!, pair.Value, pair.Key.Value(_input));
            foreach (string action in actions!)
            {
                _actionsState.Add(action, pair.Value);
            }
        }

        return _triggeredShortcuts.Count > 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private InputState GetShortcutState(Shortcut shortcut)
    {
        if (shortcut.IsPressed(_input))
        {
            return InputState.Pressed | InputState.Down;
        }

        if (shortcut.IsDown(_input))
        {
            return InputState.Down;
        }

        if (shortcut.IsReleased(_input))
        {
            return InputState.Released;
        }

        return InputState.Up;
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

    private void ConsumeInput()
    {
        if (_layers.Stack.Count == 0)
        {
            goto clear;
        }

        for (int i = _layers.Stack.Count - 1; i >= 0; i--)
        {
            var receiver = _layers.Stack[i];
            if (receiver is null)
            {
                continue;
            }

            foreach (var entry in _unconsumed)
            {
                var shortcut = _queue[entry.Index];
                if (shortcut.Consumed)
                {
                    continue;
                }

                if (!receiver.TryGetBinding(entry.Name, out var binding) || (binding.State & shortcut.State) == 0)
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

            if (receiver.ConsumesAllInput)
            {
                goto clear;
            }
        }

        clear:
        _unconsumed.Clear();
        _queue.Clear();
    }

    #endregion Private
}