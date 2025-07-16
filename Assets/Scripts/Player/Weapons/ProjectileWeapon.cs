using UnityEngine;

public abstract class ProjectileWeapon : RangedWeapon
{
    [Header("Projectile Stats")]
    [SerializeField] private float projectileSpeed;

    [Header("References")]
    [SerializeField] private Transform projectileSpawnPointTransform;
    [SerializeField] private GameObject projectileGameObject;


    public override bool Shoot(Vector3 direction) {
        if (!base.Shoot(direction)) {
            return false;
        }

        GameObject newProjectileGO = Instantiate(projectileGameObject, projectileSpawnPointTransform.position, Quaternion.identity);
        Projectile newProjectile = newProjectileGO.GetComponent<Projectile>();
        Debug.Log("initialise!");
        newProjectile.Initialise(this, direction, projectileSpeed);

        return true;
    }

    public abstract void ResolveProjectileCollision(Vector3 position, GameObject gameObjectHit);
}
