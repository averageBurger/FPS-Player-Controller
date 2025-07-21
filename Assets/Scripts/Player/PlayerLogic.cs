using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerLogic : MonoBehaviour, IStatusAffectable
{
    public static PlayerLogic Instance { get; private set; }

    [SerializeField] private float maxHealth;
    private float currentHealth;
    [SerializeField] private float maxStamina;
    private float currentStamina;

    [SerializeField] private float staminaRegenRate;

    private StatusEffectHandler effectHandler;

    public bool isDead { get; private set; } = false;

    private bool isInvulnerable = false;

    public event EventHandler OnHealthChanged;
    public event EventHandler OnStaminaChanged;
    public event EventHandler OnDeathStatusChanged;

    private void Awake() {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        Instance = this;

        effectHandler = GetComponent<StatusEffectHandler>();
        if (effectHandler == null ) { effectHandler = gameObject.AddComponent<StatusEffectHandler>(); }
    }

    public void TakeDamage(float damageAmount) {
        if (currentHealth == 0 || damageAmount <= 0 || isInvulnerable) {
            return;
        }

        currentHealth -= damageAmount;
        if (currentHealth <= 0) {
            currentHealth = 0;
            isDead = true;
            OnDeathStatusChanged?.Invoke(this, EventArgs.Empty);
        }
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Update() {
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();

        if (!playerMovement.IsSliding() && !playerMovement.IsDashing()) {
            RegenerateStamina();
        }
    }

    // For status effects
    void IStatusAffectable.TakeDamage(float amount)
    {
        TakeDamage(amount);
    }

    public void HealDamage(float healAmount) {
        if (currentHealth == maxHealth || healAmount <= 0) {
            return;
        }

        if (currentHealth == 0) {
            isDead = false;
            OnDeathStatusChanged?.Invoke(this, EventArgs.Empty);
        }
        currentHealth += healAmount;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ApplyStatusEffect(StatusEffect effect)
    {
        effectHandler.ApplyStatusEffect(effect);
    }

    private void RegenerateStamina() {
        if (currentStamina < maxStamina) {
            GainStamina(staminaRegenRate * Time.deltaTime);
        }
    }

    public bool HasEffect<E>() where E : StatusEffect
    {
        return effectHandler.HasEffect<E>();
    }

    public IReadOnlyList<StatusEffect> GetActiveStatusEffects()
    {
        return effectHandler.ActiveEffects;
    }

    public float GetCurrentHealth() { return currentHealth; }


    public void DrainStamina(float drainAmount) {
        if (currentStamina == 0 || drainAmount <= 0) {
            return;
        }

        currentStamina -= drainAmount;
        if (currentStamina < 0) {
            currentStamina = 0;
        }
        OnStaminaChanged?.Invoke(this, EventArgs.Empty);
    }

    public void GainStamina(float gainAmount) {
        if (currentStamina == maxStamina || gainAmount <= 0) {
            return;
        }

        currentStamina += gainAmount;
        if (currentStamina > maxStamina) {
            currentStamina = maxStamina;
        }
        OnStaminaChanged?.Invoke(this, EventArgs.Empty);
    }

    public float GetCurrentStamina() { return currentStamina; }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetMaxStamina()
    {
        return maxStamina;
    }

    public void SetInvulnerability(bool isInvuln) { isInvulnerable = isInvuln; }

    public bool IsDead(){ return isDead; }
}
