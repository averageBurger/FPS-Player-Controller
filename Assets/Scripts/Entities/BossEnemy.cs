using UnityEngine;

public class BossEnemy : Entity
{
    protected float primaryAttackDamage = 50f;
    protected float primaryAttackRange = 5f;
    protected float primaryAttackCooldown = 4f;

    protected float secondaryAttackDamage = 80f;
    protected float secondaryAttackRange = 10f;
    protected float secondaryAttackCooldown = 8f;

    protected bool hasSecondStage = false;
    protected float secondStageHealthThreshold = 0.5f; // Half health

    protected bool followsTarget = true;
    protected bool alwaysSeesTarget = true;

    // Getters
    public float GetPrimaryAttackDamage() => primaryAttackDamage;
    public float GetPrimaryAttackRange() => primaryAttackRange;
    public float GetPrimaryAttackCooldown() => primaryAttackCooldown;

    public float GetSecondaryAttackDamage() => secondaryAttackDamage;
    public float GetSecondaryAttackRange() => secondaryAttackRange;
    public float GetSecondaryAttackCooldown() => secondaryAttackCooldown;

    public bool HasSecondStage => hasSecondStage;
    public float SecondStageHealthThreshold => secondStageHealthThreshold;

    public bool FollowsTarget => followsTarget;
    public bool AlwaysSeesTarget => alwaysSeesTarget;

    // Setters
    public void SetPrimaryAttackDamage(float value) => primaryAttackDamage = value;
    public void SetPrimaryAttackRange(float value) => primaryAttackRange = value;
    public void SetPrimaryAttackCooldown(float value) => primaryAttackCooldown = value;

    public void SetSecondaryAttackDamage(float value) => secondaryAttackDamage = value;
    public void SetSecondaryAttackRange(float value) => secondaryAttackRange = value;
    public void SetSecondaryAttackCooldown(float value) => secondaryAttackCooldown = value;

    public void SetHasSecondStage(bool value) => hasSecondStage = value;
    public void SetSecondStageHealthThreshold(float value) => secondStageHealthThreshold = value;

    public void SetFollowsTarget(bool value) => followsTarget = value;
    public void SetAlwaysSeesTarget(bool value) => alwaysSeesTarget = value;
}
