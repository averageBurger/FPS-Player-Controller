using UnityEngine;

// Contains the base functionality of an entity
// Typically used and expanded upon by enemies
// Can be used for any "living" object
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

    public float GetHealth() { return Health; }

    public float GetMovementSpeed() { return MovementSpeed; }

    public void SetHealth(float value) { Health = value; }

    public void SetMovementSpeed(float value) { MovementSpeed = value; }

    public virtual void Kill()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"{gameObject.name} has died!");

        // Trigger death animation
        Destroy(gameObject);
    }

    // Can be made if needed at any point
    protected virtual void OnHit()
    {
        // TODO: On enemy being hit related functionality
    }

    protected virtual void OnSpawn()
    {
        // TODO: On enemy spawning in related functionality
    }
}
