using System.Collections.ObjectModel;
using RedHerring.Alexandria.Extensions;

namespace RedHerring.Fingerprint.Shortcuts;

public class CompositeShortcut : Collection<Shortcut>, Shortcut
{
    public CompositeShortcutEvaluation Evaluation;
    
    public CompositeShortcut(CompositeShortcutEvaluation evaluation = CompositeShortcutEvaluation.Conjunction)
    {
        Evaluation = evaluation;
    }

    protected override void InsertItem(int index, Shortcut? item)
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

    protected override void SetItem(int index, Shortcut? item)
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

    public static CompositeShortcut operator +(CompositeShortcut lhs, Shortcut rhs)
    {
        lhs.Add(rhs);
        return lhs;
    }

    public void InputCodes(IList<InputCode> result)
    {
        foreach (var entry in this)
        {
            entry.InputCodes(result);
        }
    }

    public float Value(Input input)
    {
        return Evaluation switch
        {
            CompositeShortcutEvaluation.Conjunction => ConjunctValue(input),
            CompositeShortcutEvaluation.Disjunction => DisjunctValue(input),
            _ => 0f,
        };
    }

    public bool IsPressed(Input input)
    {
        return Evaluation switch
        {
            CompositeShortcutEvaluation.Conjunction => AreAllPressed(input),
            CompositeShortcutEvaluation.Disjunction => AreAnyPressed(input),
            _ => false,
        };
    }

    public bool IsDown(Input input)
    {
        return Evaluation switch
        {
            CompositeShortcutEvaluation.Conjunction => AreAllDown(input),
            CompositeShortcutEvaluation.Disjunction => AreAnyDown(input),
            _ => false,
        };
    }

    public bool IsReleased(Input input)
    {
        return Evaluation switch
        {
            CompositeShortcutEvaluation.Conjunction => AreAllReleased(input),
            CompositeShortcutEvaluation.Disjunction => AreAnyReleased(input),
            _ => false,
        };
    }

    private float ConjunctValue(Input input)
    {
        float result = 0f;
        foreach (var entry in Items)
        {
            float value = entry.Value(input);
            if (value.Approximately(0f))
            {
                return 0f;
            }

            result = value;
        }
        
        return result;
    }

    private float DisjunctValue(Input input)
    {
        float result = 0f;
        foreach (var entry in Items)
        {
            float value = entry.Value(input);
            if (!value.Approximately(0f))
            {
                result = value;
            }
        }
        
        return result;
    }

    #region Conjuction

    private bool AreAllPressed(Input input)
    {
        if (Items.Count == 0)
        {
            return false;
        }

        bool onePressed = true;
        for (int i = 0; i < Items.Count; i++)
        {
            var item = Items[i];
            if (!item.IsDown(input))
            {
                return false;
            }

            if (item.IsPressed(input))
            {
                onePressed = true;
            }
        }

        return onePressed;
    }

    private bool AreAllDown(Input input)
    {
        if (Items.Count == 0)
        {
            return false;
        }

        for (int i = 0; i < Items.Count; i++)
        {
            var item = Items[i];
            if (!item.IsDown(input))
            {
                return false;
            }
        }

        return true;
    }

    private bool AreAllReleased(Input input)
    {
        if (Items.Count == 0)
        {
            return false;
        }

        bool oneReleased = true;
        for (int i = 0; i < Items.Count; i++)
        {
            var item = Items[i];
            if (item.IsReleased(input))
            {
                oneReleased = true;
            }
            else if (!item.IsDown(input))
            {
                return false;
            }
        }

        return oneReleased;
    }

    #endregion Conjuction

    #region Disjunction

    private bool AreAnyPressed(Input input)
    {
        if (Items.Count == 0)
        {
            return false;
        }
        
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].IsPressed(input))
            {
                return true;
            }
        }

        return false;
    }

    private bool AreAnyDown(Input input)
    {
        if (Items.Count == 0)
        {
            return false;
        }
        
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].IsDown(input))
            {
                return true;
            }
        }

        return false;
    }

    private bool AreAnyReleased(Input input)
    {
        if (Items.Count == 0)
        {
            return false;
        }
        
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].IsReleased(input))
            {
                return true;
            }
        }

        return false;
    }
    
    #endregion Disjunction
}