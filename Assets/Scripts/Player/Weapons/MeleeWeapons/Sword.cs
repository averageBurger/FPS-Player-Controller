using UnityEngine;

public class Sword : MeleeWeapon {
    protected override void HitEnemyEffect(GameObject gameObjectHit) {
        gameObjectHit.GetComponent<Entity>().TakeDamage(damage);
    }
}
