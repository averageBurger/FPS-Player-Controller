using UnityEngine;

public class Sniper : HitscanWeapon
{
    protected override void HitEnemyEffect(bool hitWeakpoint) {
        float damageDealt = damage;
        if (hitWeakpoint) {
            damageDealt = damage * weakpointMultiplier;
        }

        //raycastHit.transform.GetComponent<???>().TakeDamage(damageDealt);
        Debug.Log("Deal Damage! : " + damageDealt);
    }
}
