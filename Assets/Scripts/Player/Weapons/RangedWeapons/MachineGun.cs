using UnityEngine;

public class MachineGun : HitscanWeapon
{
    protected override void HitEnemyEffect(GameObject gameObjectHit, bool hitWeakpoint) {
        float damageDealt = damage;
        if (hitWeakpoint) {
            damageDealt = damage * weakpointMultiplier;
        }

        gameObjectHit.GetComponent<Entity>().TakeDamage(damageDealt);
    }
}
