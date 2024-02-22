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

    public float Value(InteractionContext interactionContext)
    {
        return Evaluation switch
        {
            CompositeShortcutEvaluation.Conjunction => ConjunctValue(interactionContext),
            CompositeShortcutEvaluation.Disjunction => DisjunctValue(interactionContext),
            _ => 0f,
        };
    }

    public bool IsPressed(InteractionContext interactionContext)
    {
        return Evaluation switch
        {
            CompositeShortcutEvaluation.Conjunction => AreAllPressed(interactionContext),
            CompositeShortcutEvaluation.Disjunction => AreAnyPressed(interactionContext),
            _ => false,
        };
    }

    public bool IsDown(InteractionContext interactionContext)
    {
        return Evaluation switch
        {
            CompositeShortcutEvaluation.Conjunction => AreAllDown(interactionContext),
            CompositeShortcutEvaluation.Disjunction => AreAnyDown(interactionContext),
            _ => false,
        };
    }

    public bool IsReleased(InteractionContext interactionContext)
    {
        return Evaluation switch
        {
            CompositeShortcutEvaluation.Conjunction => AreAllReleased(interactionContext),
            CompositeShortcutEvaluation.Disjunction => AreAnyReleased(interactionContext),
            _ => false,
        };
    }

    private float ConjunctValue(InteractionContext interactionContext)
    {
        float result = 0f;
        foreach (var entry in Items)
        {
            float value = entry.Value(interactionContext);
            if (value.Approximately(0f))
            {
                return 0f;
            }

            result = value;
        }
        
        return result;
    }

    private float DisjunctValue(InteractionContext interactionContext)
    {
        float result = 0f;
        foreach (var entry in Items)
        {
            float value = entry.Value(interactionContext);
            if (!value.Approximately(0f))
            {
                result = value;
            }
        }
        
        return result;
    }

    #region Conjuction

    private bool AreAllPressed(InteractionContext interactionContext)
    {
        if (Items.Count == 0)
        {
            return false;
        }

        bool onePressed = true;
        for (int i = 0; i < Items.Count; i++)
        {
            var item = Items[i];
            if (!item.IsDown(interactionContext))
            {
                return false;
            }

            if (item.IsPressed(interactionContext))
            {
                onePressed = true;
            }
        }

        return onePressed;
    }

    private bool AreAllDown(InteractionContext interactionContext)
    {
        if (Items.Count == 0)
        {
            return false;
        }

        for (int i = 0; i < Items.Count; i++)
        {
            var item = Items[i];
            if (!item.IsDown(interactionContext))
            {
                return false;
            }
        }

        return true;
    }

    private bool AreAllReleased(InteractionContext interactionContext)
    {
        if (Items.Count == 0)
        {
            return false;
        }

        bool oneReleased = true;
        for (int i = 0; i < Items.Count; i++)
        {
            var item = Items[i];
            if (item.IsReleased(interactionContext))
            {
                oneReleased = true;
            }
            else if (!item.IsDown(interactionContext))
            {
                return false;
            }
        }

        return oneReleased;
    }

    #endregion Conjuction

    #region Disjunction

    private bool AreAnyPressed(InteractionContext interactionContext)
    {
        if (Items.Count == 0)
        {
            return false;
        }
        
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].IsPressed(interactionContext))
            {
                return true;
            }
        }

        return false;
    }

    private bool AreAnyDown(InteractionContext interactionContext)
    {
        if (Items.Count == 0)
        {
            return false;
        }
        
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].IsDown(interactionContext))
            {
                return true;
            }
        }

        return false;
    }

    private bool AreAnyReleased(InteractionContext interactionContext)
    {
        if (Items.Count == 0)
        {
            return false;
        }
        
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].IsReleased(interactionContext))
            {
                return true;
            }
        }

        return false;
    }
    
    #endregion Disjunction
}