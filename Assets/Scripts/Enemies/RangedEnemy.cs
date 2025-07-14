using UnityEngine;

// Contains the base enemy stats for a ranged enemy
// This class is a child of the enemy class and therefore inherits the base stats of a melee enemy
// Ranged enemies require more stats and those are reflected here
// If you are creating a ranged enemy ensure that it is a child of this class!
public class RangedEnemy : Enemy
{
    [Header("Ranged Stats")]
    public virtual float ProjectileSpeed { get; protected set; } = 5f;
    public virtual GameObject ProjectileObject { get; protected set; }
}
