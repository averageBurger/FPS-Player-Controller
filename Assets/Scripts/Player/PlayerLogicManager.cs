using UnityEngine;

public class PlayerLogicManager : MonoBehaviour
{
    public static PlayerLogicManager Instance { get; private set; }

    [SerializeField] private int maxHealth;
    public int currentHealth;

    [SerializeField] private int maxStamina;
    private int currentStamina;

    public bool isDead { get; private set; } = false;

    private void Awake() {
        Instance = this;
    }

    public void TakeDamage(int damageAmount) {
        if (currentHealth == 0 || damageAmount < 1) {
            return;
        }

        currentHealth -= damageAmount;
        if (currentHealth < 0) {
            currentHealth = 0;
            isDead = true;
        }
    }

    public void HealDamage(int healAmount) {
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

    public int GetCurrentHealth() { return currentHealth; }


    public void DrainStamina(int drainAmount) {
        if (currentStamina == 0 || drainAmount < 1) {
            return;
        }

        currentStamina -= drainAmount;
        if (currentStamina < 0) {
            currentStamina = 0;
        }
    }

    public void GainStamina(int gainAmount) {
        if (currentStamina == maxStamina || gainAmount < 1) {
            return;
        }

        currentStamina += gainAmount;
        if (currentStamina > maxStamina) {
            currentStamina = maxStamina;
        }
    }

    public int GetCurrentStamina() { return currentStamina; }
}
