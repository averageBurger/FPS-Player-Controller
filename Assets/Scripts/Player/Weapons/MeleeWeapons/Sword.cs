using UnityEngine;

public class Sword : MeleeWeapon {
    protected override void HitEnemyEffect(RaycastHit hit) {
        hit.transform.gameObject.GetComponent<Entity>().TakeDamage(damage);
    }
}
