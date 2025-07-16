using UnityEngine;

public class RocketLauncher : ProjectileWeapon
{
    [Header("Rocket Launcher Stats")]
    [SerializeField] private int splashDamage;
    [SerializeField] private float explosionRadius;

    public override void ResolveProjectileCollision(Vector3 projectilePosition, GameObject gameObjectHit) {
        if (WeaponManager.Instance.HasHitEnemy(gameObjectHit.layer)) {
            //gameObjectHit.GetComponent<???>().DealDamage(damage);
            Debug.Log("Deal Direct Damage: " + damage);
        }

        Collider[] nearbyObjectColliders = Physics.OverlapSphere(projectilePosition, explosionRadius, WeaponManager.Instance.GetWeaponInteractableLayerMask()); 

        foreach (Collider c in nearbyObjectColliders) {
            if (WeaponManager.Instance.HasHitEnemy(c.gameObject.layer) && IsInSplashLineOfSight(projectilePosition, c)) {
                Debug.Log("Deal splash Damage: " + splashDamage);
            }
        }
    }



    private bool IsInSplashLineOfSight(Vector3 projectilePosition, Collider c) {
        Vector3 dir = (c.transform.position - projectilePosition).normalized;
        Physics.Raycast(projectilePosition, dir, out RaycastHit raycastHit, explosionRadius, WeaponManager.Instance.GetWeaponInteractableLayerMask());
        int layer = raycastHit.transform.gameObject.layer;

        return WeaponManager.Instance.HasHitEnemy(layer);
    }
}
