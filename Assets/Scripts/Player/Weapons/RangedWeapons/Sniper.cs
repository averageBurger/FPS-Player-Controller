using UnityEngine;

public class Sniper : HitscanWeapon
{
    protected override void HitEnemyEffect(bool hitWeakpoint) {
        int damageDealt = damage;
        if (hitWeakpoint) {
            damageDealt = (int)(damage * weakpointMultiplier);
        }

        //raycastHit.transform.GetComponent<???>().TakeDamage(damageDealt);
        Debug.Log("Deal Damage! : " + damageDealt);
    }
}
