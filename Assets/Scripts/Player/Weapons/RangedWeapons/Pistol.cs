using System;
using System.Collections;
using UnityEngine;

// CREATED BY: Sean
// Script for the pistol. Hitscan weapon, so use a raycast to find out what it hits.
public class Pistol : HitscanWeapon
{
    protected override void HitEnemyEffect(GameObject gameObjectHit, bool hitWeakpoint) {
        float damageDealt = damage;
        if (hitWeakpoint) {
            damageDealt = damage * weakpointMultiplier;
        }

        gameObjectHit.GetComponent<Entity>().TakeDamage(damageDealt);
    }
}
