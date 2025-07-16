using System;
using System.Collections;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

// CREATED BY: Sean
// Script for the pistol. Hitscan weapon, so use a raycast to find out what it hits.
public class Pistol : HitscanWeapon
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
