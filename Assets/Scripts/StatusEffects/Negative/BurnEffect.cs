using UnityEngine;

public class BurnEffect : StatusEffect
{
    private float damagePerSecond = 5f;
    private StatusEffectHandler handler;

    public BurnEffect(float duration) : base(duration) { }

    protected override void OnInterval()
    {
        var affectableTarget = handler.GetComponent<IStatusAffectable>();
        if (affectableTarget != null)
        {
            affectableTarget.TakeDamage(damagePerSecond);
        }
    }

    public override void Apply(StatusEffectHandler handler)
    {
        this.handler = handler;
        // particles etc
    }

    public override void Remove(StatusEffectHandler handler)
    {
    }
}
