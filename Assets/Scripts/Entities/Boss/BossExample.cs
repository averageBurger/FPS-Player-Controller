using UnityEngine;

public class BossExample : BossEnemy
{
    // Only serialize fields that have been overriden
    // This makes sense because we shouldn't be serializing stats that aren't being changed...
    // ...from what is defined in the parent classes
    [SerializeField] private float bossMaxHealth = 2000f;
    [SerializeField] private float bossMovementSpeed = 9f;
    [SerializeField] private float bossPrimaryAttackDamage = 50f;
    [SerializeField] private bool bossHasSecondStage = true;
    [SerializeField] private bool bossAlwaysSeesTarget = false;

    private new void Awake()
    {
        SetHealth(bossMaxHealth);
        SetMovementSpeed(bossMovementSpeed);
        SetPrimaryAttackDamage(bossPrimaryAttackDamage);
        SetHasSecondStage(bossHasSecondStage);
        SetAlwaysSeesTarget(bossAlwaysSeesTarget);
    }
}
