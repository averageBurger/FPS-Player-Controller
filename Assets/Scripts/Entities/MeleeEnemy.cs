using UnityEngine;

// Contains the base melee enemy stats for melee enemy classes to override to their needs
// All melee enemies depend on this class to function, anything changed here affects all melee enemies!
// When creating an enemy ensure that the class is a child of either this class or the RangedEnemy class if ranged
public class MeleeEnemy : Entity
{
    protected float attackDamage = 20f;
    protected float attackRange = 2f;
    protected float attackCooldown = 2f;

    public float GetAttackDamage() => attackDamage;
    public void SetAttackDamage(float value) => attackDamage = value;

    public float GetAttackRange() => attackRange;
    public void SetAttackRange(float value) => attackRange = value;

    public float GetAttackCooldown() => attackCooldown;
    public void SetAttackCooldown(float value) => attackCooldown = value;
}