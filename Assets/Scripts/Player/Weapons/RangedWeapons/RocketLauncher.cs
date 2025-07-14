using UnityEngine;

public class RocketLauncher : ProjectileWeapon
{
    [Header("Rocket Launcher Stats")]
    [SerializeField] private int splashDamage;

    public override void ResolveProjectileCollision(GameObject gameObjectHit) {
        if (WeaponManager.Instance.HasHitEnemy(gameObjectHit.layer)) {
            //gameObjectHit.GetComponent<???>().DealDamage(damage);
            Debug.Log("Deal Direct Damage: " + damage);
        }

        // AOE effect
    }
}
