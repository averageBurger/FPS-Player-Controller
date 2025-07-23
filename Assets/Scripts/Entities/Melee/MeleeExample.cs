using Unity.VisualScripting;
using UnityEngine;

// Override stats in this class
public class MeleeExample : MeleeEnemy
{
    // Only serialize fields that have been overriden
    // This makes sense because we shouldn't be serializing stats that aren't being changed...
    // ...from what is defined in the parent classes
    [Header("Melee Example")]
    [SerializeField] private float enemyMaxHealth = 100f;
    [SerializeField] private float enemyAttackDamage = 15f;
    [SerializeField] private float enemyAttackCooldown = 1f;
    [SerializeField] private float enemyMovementSpeed = 13f;

    protected override void Awake()
    {
        // Override the base values
        SetHealth(enemyMaxHealth); 
        SetAttackDamage(enemyAttackDamage);
        SetAttackCooldown(enemyAttackCooldown);
        SetMovementSpeed(enemyMovementSpeed);
    }
}
