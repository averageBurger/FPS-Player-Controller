using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    ProjectileWeapon projectileWeaponParent;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    public void Initialise(ProjectileWeapon sender, Vector3 dir, float speed) {
        Debug.Log("received!");
        projectileWeaponParent = sender;
        rb.AddRelativeForce(dir * speed, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision) {
        GameObject gameObjectHit = collision.gameObject;
        projectileWeaponParent.ResolveProjectileCollision(transform.position, gameObjectHit);
        Destroy(gameObject);
    }
}
