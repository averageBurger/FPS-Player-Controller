using UnityEngine;

public class BlindEffect : StatusEffect
{
    private StatusEffectHandler handler;

    public BlindEffect(float duration) : base(duration)
    {
    }

    public override void Apply(StatusEffectHandler handler)
    {
        // particles etc
    }

    public override void Remove(StatusEffectHandler handler)
    {
    }
}
