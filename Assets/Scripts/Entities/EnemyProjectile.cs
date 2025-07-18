using UnityEngine;

public interface IProjectile
{
    void Spawn(Vector3 direction);
    void SetTarget(Transform target);
}

[RequireComponent(typeof(Rigidbody))]
public abstract class EnemyProjectile : MonoBehaviour, IProjectile
{
    [Header("Base Projectile Stats")]
    public virtual float ProjectileDamage { get; protected set; } = 40f;
    public virtual float ProjectileSpeed { get; protected set; } = 8f;

    protected Vector3 direction;
    protected Rigidbody rb;
    protected Transform target;

    public virtual void Spawn(Vector3 direction)
    {
        rb = GetComponent<Rigidbody>();
        this.direction = direction.normalized;

        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.linearVelocity = this.direction * ProjectileSpeed;
    }

    public float GetDamage() => ProjectileDamage;

    public float GetSpeed() => ProjectileSpeed;

    // Likely to have no use since the target is probably always going to be the player
    // Remove if not used
    public Transform GetTarget() { return target; }

    public void SetDamage(float damage) => ProjectileDamage = damage;

    public void SetSpeed(float speed) => ProjectileSpeed = speed;

    public virtual void SetTarget(Transform target) { this.target = target; }
}