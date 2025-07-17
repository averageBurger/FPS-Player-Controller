using UnityEngine;

public interface IProjectile
{
    void Spawn(Vector3 direction);
}

[RequireComponent(typeof(Rigidbody))]
public abstract class Projectile : MonoBehaviour, IProjectile
{
    [Header("Base Projectile Stats")]
    public virtual float ProjectileDamage { get; protected set; } = 40f;
    public virtual float ProjectileSpeed { get; protected set; } = 8f;

    protected Vector3 direction;
    protected Rigidbody rb;

    public virtual void Spawn(Vector3 direction)
    {
        rb = GetComponent<Rigidbody>();
        this.direction = direction.normalized;

        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.linearVelocity = this.direction * ProjectileSpeed;
    }

    public float GetDamage() => ProjectileDamage;
    public float GetSpeed() => ProjectileSpeed;

    public void SetDamage(float damage) => ProjectileDamage = damage;
    public void SetSpeed(float speed) => ProjectileSpeed = speed;
}