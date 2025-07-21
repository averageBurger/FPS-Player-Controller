using System.Collections.Generic;
using UnityEngine;

public class PlayerLogicManager : MonoBehaviour, IStatusAffectable
{
    public static PlayerLogicManager Instance { get; private set; }

    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxStamina;
    [SerializeField] private float currentStamina;

    private StatusEffectHandler effectHandler;

    public bool isDead { get; private set; } = false;

    private void Awake() {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        Instance = this;

        effectHandler = GetComponent<StatusEffectHandler>();
        if (effectHandler == null ) { effectHandler = gameObject.AddComponent<StatusEffectHandler>(); }
    }

    public void TakeDamage(float damageAmount) {
        if (currentHealth == 0 || damageAmount < 1) {
            return;
        }

        currentHealth -= damageAmount;
        if (currentHealth < 0) {
            currentHealth = 0;
            isDead = true;
        }
    }

    // For status effects
    void IStatusAffectable.TakeDamage(float amount)
    {
        TakeDamage(amount);
    }

    public void HealDamage(float healAmount) {
        if (currentHealth == maxHealth || healAmount < 1) {
            return;
        }

        if (currentHealth == 0) {
            isDead = false;
        }
        currentHealth += healAmount;
        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
    }

    public void ApplyStatusEffect(StatusEffect effect)
    {
        effectHandler.ApplyStatusEffect(effect);
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
        if (currentStamina == 0 || drainAmount < 1) {
            return;
        }

        currentStamina -= drainAmount;
        if (currentStamina < 0) {
            currentStamina = 0;
        }
    }

    public void GainStamina(float gainAmount) {
        if (currentStamina == maxStamina || gainAmount < 1) {
            return;
        }

        currentStamina += gainAmount;
        if (currentStamina > maxStamina) {
            currentStamina = maxStamina;
        }
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
}
