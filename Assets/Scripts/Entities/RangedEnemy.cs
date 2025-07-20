using UnityEngine;

// Contains the base enemy stats for a ranged enemy
// If you are creating a ranged enemy, ensure that it is a child of this class!
public class RangedEnemy : Entity
{
    protected GameObject projectileObject;
    protected Transform projectileFirePoint;
    protected float reloadCooldown = 3f;

    public GameObject GetProjectileObject() => projectileObject;
    public void SetProjectileObject(GameObject projectile) => projectileObject = projectile;

    public Transform GetProjectileFirePoint() => projectileFirePoint;
    public void SetProjectileFirePoint(Transform firePoint) => projectileFirePoint = firePoint;

    public float GetReloadCooldown() => reloadCooldown;
    public void SetReloadCooldown(float value) => reloadCooldown = value;
}
