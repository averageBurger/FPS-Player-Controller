using UnityEngine;

// Override stats in this class
public class RangedExample : RangedEnemy
{
    // Only serialize fields that have been overriden
    // This makes sense because we shouldn't be serializing stats that aren't being changed...
    // ...from what is defined in the parent classes
    [SerializeField] private GameObject enemyProjectileObj;
    [SerializeField] private Transform enemyProjectileFirePoint;

    [SerializeField] private float enemyMaxHealth = 80f;
    [SerializeField] private float enemyReloadCooldown = 2.5f;

    private void Awake()
    {
        // Override base values
        SetProjectileObject(enemyProjectileObj);
        SetProjectileFirePoint(enemyProjectileFirePoint);
        SetHealth(enemyMaxHealth);
        SetReloadCooldown(enemyReloadCooldown);
    }
}
