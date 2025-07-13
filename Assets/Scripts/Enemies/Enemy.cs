using UnityEngine;

// Contains the base enemy stats for enemy classes to override to their needs
// When creating an enemy ensure that the class is a child of this class
public class Enemy : MonoBehaviour
{
    // Enemy stats
    public float maxHealth = 100f;
    public float attackDamage = 20f;
    public float movementSpeed = 10f;

    // Current health value
    protected float Health;

    protected virtual void Start()
    {
        Health = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0f)
        {
            KillEnemy();
        }
    }

    protected virtual void KillEnemy()
    {
        Destroy(gameObject);
    }

    // Used to get the enemy's attack damage in their AI class
    public virtual float GetAttackDamage()
    {
        return attackDamage;
    }
}