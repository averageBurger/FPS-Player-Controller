using UnityEngine;

public class Sniper : RangedWeapon
{
    public override bool Shoot(Vector3 direction) {
        if (!base.Shoot(direction)) {
            return false;
        }

        if (!Physics.Raycast(WeaponManager.Instance.GetBulletSpawnPoint(), direction, out RaycastHit raycastHit, range, WeaponManager.Instance.GetRangedWeaponLayerMask())) {
            Debug.Log("hit nothing!");
            return true;
        }

        int layer = raycastHit.transform.gameObject.layer;
        if (WeaponManager.Instance.HasHitObstacle(layer)) {
            Debug.Log("Blocked!");
            return true;
        }

        int damageDealt = damage;
        if (WeaponManager.Instance.HasHitEnemyWeakpoint(layer)) {
            damageDealt = (int)(damage * weakpointMultiplier);
        }

        //raycastHit.transform.GetComponent<???>().TakeDamage();
        Debug.Log("Deal Damage! : " + damageDealt);
        return true;
    }
}
