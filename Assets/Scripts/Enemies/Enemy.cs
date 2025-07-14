using UnityEngine;

// Contains the base enemy stats for enemy classes to override to their needs as well as basic enemy functions
// All enemies depend on this class to function as an enemy, anything changed here affects all enemies!
// When creating an enemy ensure that the class is a child of either this class or the RangedEnemy class if ranged
public class Enemy : MonoBehaviour
{
    [Header("General Enemy Stats")]
    public virtual float MaxHealth { get; protected set; } = 100f;
    public virtual float AttackDamage { get; protected set; } = 20f;
    public virtual float MovementSpeed { get; protected set; } = 6f;
    public virtual float AttackCooldown { get; protected set; } = 2f;

    [Header("State")]
    protected float Health;

    protected virtual void Start()
    {
        Health = MaxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0f)
        {
            KillEnemy();
        }
    }

    public virtual void Stun(float duration)
    {
        // Maybe stun functionality?
    }

    protected virtual void OnHit()
    {
        // On enemy being hit related functionality
    }

    protected virtual void OnSpawn()
    {
        // On enemy spawning in related functionality
    }

    protected virtual void KillEnemy()
    {
        // Trigger death animation
        Destroy(gameObject);
    }
}