using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    ProjectileWeapon projectileWeaponParent;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialise(ProjectileWeapon sender, Vector3 dir, float speed) {
        projectileWeaponParent = sender;
        rb.AddRelativeForce(dir * speed, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision) {
        int layerHit = collision.gameObject.layer;
        projectileWeaponParent.ResolveProjectileCollision(layerHit);
    }
}
