using System.Collections.Generic;
using UnityEngine;

// Contains the base functionality of an entity
// Typically used and expanded upon by enemies
// Can be used for any "living" object
public class Entity : MonoBehaviour, IStatusAffectable
{
    // Unless they are overriden in their respective child class, changing these stats will affect all entities!
    private bool invulnerable = false;
    private bool looksAtTarget = true;

    private float maxHealth = 100f;
    private float movementSpeed = 6f;

    protected StatusEffectHandler effectHandler;

    [Header("Entity State")]
    [SerializeField] protected float health;
    [SerializeField] protected bool isDead = false; // Not necessary unless the game object is not destroyed

    public bool LooksAtTarget => looksAtTarget;
    public float MaxHealth => maxHealth;

    protected virtual void Awake()
    {
        effectHandler = GetComponent<StatusEffectHandler>();
    }

    protected virtual void Start()
    {
        health = MaxHealth;
    }

    protected virtual void Update()
    {
        if (isDead) { Kill(); }
    }

    public virtual void TakeDamage(float damage)
    {
        if(isDead || invulnerable == true) return;

        health -= damage;
        if (health <= 0f)
        {
            Kill();
        }
    }

    // For status effects
    void IStatusAffectable.TakeDamage(float amount)
    {
        TakeDamage(amount);
    }

    public virtual void Kill()
    {
        if (invulnerable == true) { return; }
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

    public bool HasEffect<E>() where E : StatusEffect
    {
        return effectHandler.HasEffect<E>();
    }

    public float GetHealth() => health;
    public float GetMovementSpeed() => movementSpeed;
    public bool GetInvulnerability() => invulnerable;

    public IReadOnlyList<StatusEffect> GetActiveStatusEffects()
    {
        return effectHandler.ActiveEffects;
    }

    public void SetHealth(float value) => health = value;
    public void SetMovementSpeed(float value) => movementSpeed = value;
    public void SetInvulnerability(bool state) => invulnerable = state;

    public void ApplyStatusEffect(StatusEffect effect)
    {
        if (invulnerable) { return; }
        effectHandler.ApplyStatusEffect(effect);
    }
}