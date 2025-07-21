using UnityEngine;

public class StunEffect : StatusEffect
{
    private StatusEffectHandler handler;
    private Entity entity;

    public StunEffect(float duration) : base(duration)
    {
    }

    public override void Apply(StatusEffectHandler handler)
    {
        // handler.GetComponent<IStatusAffectable>()?.SetStunned(true);

        // particles etc
    }

    public override void Remove(StatusEffectHandler handler)
    {
    }
}