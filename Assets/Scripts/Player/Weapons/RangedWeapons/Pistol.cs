using JetBrains.Annotations;
using UnityEngine;

// CREATED BY: Sean
// Script for the pistol. Hitscan weapon, so use a raycast to find out what it hits.
public class Pistol : RangedWeapon
{

    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] private float headShotMultiplier;

    public override void Shoot(Vector3 startPos, Vector3 direction) {
        if (!Physics.Raycast(startPos, direction, out RaycastHit raycastHit, range, WeaponManager.Instance.GetRangedWeaponLayerMask())) {
            Debug.Log("hit nothing!");
            return;
        }

        int layer = raycastHit.transform.gameObject.layer;
        // layer is weird binary. Bit shift obstacle value by layer number, check if it equals 1
        bool hitObstacle = 1 == WeaponManager.Instance.GetObstacleLayer().value >> layer;
        if (hitObstacle) {
            Debug.Log("Blocked!");
            return;
        }

        bool hitWeakpoint = 1 == WeaponManager.Instance.GetEnemyWeakpointLayer().value >> layer;
        int damageDealt = damage;
        if (hitWeakpoint) {
            damageDealt = (int) (damage * headShotMultiplier);
        }

        //raycastHit.transform.GetComponent<???>().TakeDamage();
        Debug.Log("Deal Damage! : " + damageDealt);
    }
}
