using UnityEngine;

// Contains the base functionality of an entity
// Typically used and expanded upon by melee and ranged enemies
public class Entity : MonoBehaviour
{
    [Header("Base Entity Stats")]
    public virtual float MaxHealth { get; protected set; } = 100f;
    public virtual float MovementSpeed { get; protected set; } = 6f;

    [Header("State")]
    protected float Health;
    protected bool isDead = false;

    protected virtual void Start()
    {
        Health = MaxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        if(isDead) return;

        Health -= damage;
        if (Health <= 0f)
        {
            Kill();
        }
    }

    protected virtual void OnHit()
    {
        // On enemy being hit related functionality
    }

    protected virtual void OnSpawn()
    {
        // On enemy spawning in related functionality
    }

    protected virtual void Kill()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"{gameObject.name} has died!");

        // Trigger death animation
        Destroy(gameObject);
    }
}
