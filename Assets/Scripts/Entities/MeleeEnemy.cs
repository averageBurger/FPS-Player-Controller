using UnityEngine;

// Contains the base melee enemy stats for melee enemy classes to override to their needs
// All melee enemies depend on this class to function, anything changed here affects all melee enemies!
// When creating an enemy ensure that the class is a child of either this class or the RangedEnemy class if ranged
public class MeleeEnemy : Entity
{
    [Header("Melee Attack")]
    public virtual float AttackDamage { get; protected set; } = 20f;
    public virtual float AttackRange { get; protected set; } = 2f;
    public virtual float AttackCooldown { get; protected set; } = 2f;

    public virtual void Stun(float duration)
    {
        // Maybe stun functionality?
    }
}