using UnityEngine;

public class Sword : MeleeWeapon {
    protected override void HitEnemyEffect() {
        // deal damage to enemy
        Debug.Log("Hit enemy for " + damage + " damage!");
    }
}
