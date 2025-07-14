using UnityEngine;

public class RangedExample : RangedEnemy
{
    public GameObject projectileObj;

    public override float MaxHealth => 80f;
    public override float AttackDamage => 40f;
    public override float AttackCooldown => 3f;

    public override float ProjectileSpeed => 10f;
    public override GameObject ProjectileObject => projectileObj;
    // Must be overriden to assign an object or else an error will occur, this should be addressed
}
