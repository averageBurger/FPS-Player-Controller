using UnityEngine;

// Contains the base enemy stats for a ranged enemy
// If you are creating a ranged enemy ensure that it is a child of this class!
public class RangedEnemy : Entity
{
    [Header("Projectile Attack")]
    public virtual GameObject ProjectileObject { get; protected set; }
    public virtual Transform ProjectileFirePoint { get; protected set; }
    public virtual float ReloadCooldown { get; protected set; } = 3f;
}
