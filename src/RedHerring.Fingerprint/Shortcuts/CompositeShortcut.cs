using System.Collections.ObjectModel;
using RedHerring.Alexandria.Extensions;

namespace RedHerring.Fingerprint.Shortcuts;

public class CompositeShortcut : Collection<IShortcut>, IShortcut
{
    public CompositeShortcutEvaluation Evaluation;
    
    public CompositeShortcut(CompositeShortcutEvaluation evaluation = CompositeShortcutEvaluation.Conjunction)
    {
        Evaluation = evaluation;
    }

    protected override void InsertItem(int index, IShortcut? item)
    {
        if (item is null)
        {
            return;
        }

        if (!Contains(item))
        {
            base.InsertItem(index, item);
        }
    }

    protected override void SetItem(int index, IShortcut? item)
    {
        if (item is null)
        {
            return;
        }

        if (!Contains(item))
        {
            base.SetItem(index, item);
        }
    }

    public static CompositeShortcut operator +(CompositeShortcut lhs, IShortcut rhs)
    {
        lhs.Add(rhs);
        return lhs;
    }

    public void GetInputCodes(IList<InputCode> result)
    {
        foreach (var entry in this)
        {
            entry.GetInputCodes(result);
        }
    }

    public float GetValue(Input input)
    {
        return Evaluation switch
        {
            CompositeShortcutEvaluation.Conjunction => GetConjunctValue(input),
            CompositeShortcutEvaluation.Disjunction => GetDisjunctValue(input),
            _ => 0f,
        };
    }

    public bool IsUp(Input input)
    {
        return Evaluation switch
        {
            CompositeShortcutEvaluation.Conjunction => Items.All(e => e.IsUp(input)),
            CompositeShortcutEvaluation.Disjunction => Items.Any(e => e.IsUp(input)),
            _ => false,
        };
    }

    public bool IsPressed(Input input)
    {
        return Evaluation switch
        {
            CompositeShortcutEvaluation.Conjunction => Items.All(e => e.IsPressed(input)),
            CompositeShortcutEvaluation.Disjunction => Items.Any(e => e.IsPressed(input)),
            _ => false,
        };
    }

    public bool IsDown(Input input)
    {
        return Evaluation switch
        {
            CompositeShortcutEvaluation.Conjunction => Items.All(e => e.IsDown(input)),
            CompositeShortcutEvaluation.Disjunction => Items.Any(e => e.IsDown(input)),
            _ => false,
        };
    }

    public bool IsReleased(Input input)
    {
        return Evaluation switch
        {
            CompositeShortcutEvaluation.Conjunction => Items.All(e => e.IsReleased(input)),
            CompositeShortcutEvaluation.Disjunction => Items.Any(e => e.IsReleased(input)),
            _ => false,
        };
    }

    private float GetConjunctValue(Input input)
    {
        float result = 0f;
        foreach (var entry in Items)
        {
            float value = entry.GetValue(input);
            if (value.Approximately(0f))
            {
                return 0f;
            }

            result = value;
        }
        
        return result;
    }

    private float GetDisjunctValue(Input input)
    {
        float result = 0f;
        foreach (var entry in Items)
        {
            float value = entry.GetValue(input);
            if (!value.Approximately(0f))
            {
                result = value;
            }
        }
        
        return result;
    }
}