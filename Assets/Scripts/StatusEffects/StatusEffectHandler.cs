using System.Collections.Generic;
using UnityEngine;

public class StatusEffectHandler : MonoBehaviour
{
    private List<StatusEffect> activeEffects = new();

    public IReadOnlyList<StatusEffect> ActiveEffects => activeEffects.AsReadOnly();

    // To update the list of active status effects
    private void Update()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].CalculateInterval(Time.deltaTime);
            if (activeEffects[i].IsFinished)
            {
                activeEffects[i].Remove(this);
                activeEffects.RemoveAt(i);
            }
        }
    }

    // To add a status effect to a player/entity
    public void ApplyStatusEffect(StatusEffect newAppliedEffect)
    {
        // Prevents duplicate effects
        foreach (var effect in activeEffects)
        {
            if (effect.GetType() == newAppliedEffect.GetType())
            {
                effect.Refresh(newAppliedEffect.duration);
                return;
            }
        }
        activeEffects.Add(newAppliedEffect);
        newAppliedEffect.Apply(this);
    }

    // A check for whether the player/entity has an effect
    public bool HasEffect<E>() where E : StatusEffect
    {
        foreach (var effect in activeEffects) 
        { 
            if (effect is E)
                return true;
        }
        return false;
    }
}
