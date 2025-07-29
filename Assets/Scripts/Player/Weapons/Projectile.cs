using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    ProjectileWeapon projectileWeaponParent;

    private float despawnTimer = 0f;
    private float maxTime = 20f;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        despawnTimer += Time.deltaTime;
        if (despawnTimer > maxTime) {
            Destroy(gameObject);
        }
    }

    public void Initialise(ProjectileWeapon sender, Vector3 dir, float speed) {
        projectileWeaponParent = sender;
        rb.AddRelativeForce(dir * speed, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision) {
        GameObject gameObjectHit = collision.gameObject;
        projectileWeaponParent.ResolveProjectileCollision(transform.position, gameObjectHit);
        Destroy(gameObject);
    }
}
