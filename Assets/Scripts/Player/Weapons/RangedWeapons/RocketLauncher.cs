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

        Collider[] nearbyObjectColliders = Physics.OverlapSphere(projectilePosition, explosionRadius, WeaponManager.Instance.GetRangedWeaponLayerMask()); 

        foreach (Collider c in nearbyObjectColliders) {
            if (WeaponManager.Instance.HasHitEnemy(c.gameObject.layer) && IsInSplashLineOfSight(projectilePosition, c)) {
                Debug.Log("Deal splash Damage: " + splashDamage);
            }
        }
    }



    private bool IsInSplashLineOfSight(Vector3 projectilePosition, Collider c) {
        Debug.Log(c.gameObject.name);

        Vector3 dir = (c.transform.position - projectilePosition).normalized;
        Physics.Raycast(projectilePosition, dir, out RaycastHit raycastHit, explosionRadius, WeaponManager.Instance.GetRangedWeaponLayerMask());
        int layer = raycastHit.transform.gameObject.layer;
        if (!WeaponManager.Instance.HasHitEnemy(layer)) {
            Debug.Log("non enemy in the way");
            return false;
        }

        return true;
    }
}
