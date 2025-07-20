using System.Collections;
using UnityEngine;

// Contains the base functionality of an entity
// Typically used and expanded upon by enemies
// Can be used for any "living" object
public class Entity : MonoBehaviour
{
    // Unless they are overriden in their respective child class, changing these stats will affect all entities!
    private bool isBoss = false;
    private bool invulnerable = false;
    private bool looksAtTarget = true;

    private float maxHealth = 100f;
    private float movementSpeed = 6f;

    public bool IsBoss => isBoss;
    public bool LooksAtTarget => looksAtTarget;

    public float MaxHealth => maxHealth;
    public float GetHealth() => health;
    public float GetMovementSpeed() => movementSpeed;
    public bool GetInvulnerability() => invulnerable;

    public void SetHealth(float value) => health = value;
    public void SetMovementSpeed(float value) => movementSpeed = value;
    public void SetInvulnerability(bool state) => invulnerable = state;

    [Header("State")]
    protected float health;
    protected bool isDead = false;
    protected bool isStunned = false;
    protected bool isOnFire = false;
    protected bool isBlind = false;

    private float stunTimer = 0f;
    private float blindTimer = 0f;

    protected virtual void Start()
    {
        health = MaxHealth;
    }

    protected virtual void Update()
    {
        EffectTimer(stunTimer, isStunned);
        EffectTimer(blindTimer, isBlind);
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

    public virtual void Kill()
    {
        if (isDead || invulnerable == true) { return; }
        isDead = true;

        Debug.Log($"{gameObject.name} has died!");

        // Trigger death animation
        Destroy(gameObject);
    }

    public virtual void Stun(float duration)
    {
        if (isDead || invulnerable == true) { return; }

        isStunned = true;
        stunTimer = duration;
    }

    public virtual void Blind(float duration)
    {
        if (isDead || invulnerable == true) { return; }

        isBlind = true;
        blindTimer = duration;
    }

    public virtual void SetOnFire(float damagePerSecond, float duration)
    {
        if (isDead || invulnerable == true) { return; }

        // TODO: Indicate that the entity is on fire

        isOnFire = true;
        StartCoroutine(ApplyBurnEffect(damagePerSecond, duration));
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

    private void EffectTimer(float timer, bool effectState)
    {
        if (effectState)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                effectState = false;
            }
        }
    }

    private IEnumerator ApplyBurnEffect(float damagePerSecond, float duration)
    {
        float burnInterval = 1f; 
        float timeElapsed = 0f;

        while (timeElapsed < duration) 
        { 
            if (isDead || invulnerable == true) { break; }

            TakeDamage(damagePerSecond);

            yield return new WaitForSeconds(burnInterval);
            timeElapsed += burnInterval;
        }

        isOnFire = false;
    }
}