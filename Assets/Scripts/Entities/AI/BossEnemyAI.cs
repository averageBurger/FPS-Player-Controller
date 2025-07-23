using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// Contains the boss behaviours and related functions for individual bosses to inherit and override
// Handles dealing damage, primary and secondary attacks, alongside the second stage if enabled
// All bosses share these behaviours
public abstract class BossEnemyAI : EnemyAI
{
    protected BossEnemy bossEnemy;
    protected NavMeshAgent navAgent;

    protected float primaryAttackCooldownTimer = 0f;
    protected float secondaryAttackCooldownTimer = 0f;

    protected bool inSecondStage = false;

    private int primaryAttackCount = 0;
    private bool isPerformingAttack = false;
    private enum AttackPhase { Primary, Secondary }
    private AttackPhase currentPhase = AttackPhase.Primary;

    protected new virtual void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        bossEnemy = GetComponent<BossEnemy>();

        navAgent.speed = bossEnemy.GetMovementSpeed();
    }

    protected new virtual void Update()
    {
        if (bossEnemy == null || target == null || bossEnemy.GetHealth() <= 0 || bossEnemy.GetInvulnerability()) return;

        // We are still raycasting even if they always see the target so this could implemented better
        if (bossEnemy.LooksAtTarget && bossEnemy.AlwaysSeesTarget || CanSeeTarget())
        {
            LookAtTarget();
        }

        if (bossEnemy.FollowsTarget && CanSeeTarget() && navAgent != null)
        {
            navAgent.isStopped = false;
            navAgent.SetDestination(target.position);
        }
        else if (bossEnemy.AlwaysSeesTarget && navAgent != null)
        {
            navAgent.isStopped = false;
            navAgent.SetDestination(target.position);
        }
        else if (navAgent != null)
        {
            navAgent.isStopped = true;
        }

        // Ideally we don't want to be checking this every frame but if the performance hit is minimal
        // then it should be relatively fine considering the fact that there should be only 1 boss at once
        HandleSecondStage();

        primaryAttackCooldownTimer -= Time.deltaTime;
        secondaryAttackCooldownTimer -= Time.deltaTime;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // Attacks work in a combo: Primary -> Primary -> Secondary (...and repeat)
        if (!isPerformingAttack)
        {
            switch (currentPhase)
            {
                case AttackPhase.Primary:
                    if (distanceToTarget <= bossEnemy.GetPrimaryAttackRange() && primaryAttackCooldownTimer <= 0f)
                    {
                        StartCoroutine(PerformPrimaryAttack());
                    }
                    break;

                case AttackPhase.Secondary:
                    if (distanceToTarget <= bossEnemy.GetSecondaryAttackRange() && secondaryAttackCooldownTimer <= 0f)
                    {
                        StartCoroutine(PerformSecondaryAttack());
                    }
                    break;
            }
        }
    }

    private IEnumerator PerformPrimaryAttack()
    {
        isPerformingAttack = true;
        PrimaryAttack(); // Attack logic
        primaryAttackCooldownTimer = bossEnemy.GetPrimaryAttackCooldown();

        primaryAttackCount++;

        // Wait for the cooldown to end before allowing next attack
        yield return new WaitForSeconds(bossEnemy.GetPrimaryAttackCooldown());

        if (primaryAttackCount >= 2)
        {
            currentPhase = AttackPhase.Secondary;
            primaryAttackCount = 0;
        }

        isPerformingAttack = false;
    }

    private IEnumerator PerformSecondaryAttack()
    {
        isPerformingAttack = true;
        SecondaryAttack();
        secondaryAttackCooldownTimer = bossEnemy.GetSecondaryAttackCooldown();

        yield return new WaitForSeconds(bossEnemy.GetSecondaryAttackCooldown());

        currentPhase = AttackPhase.Primary;
        isPerformingAttack = false;
    }

    // If health is below the threshold for the second stage we enter the second stage
    protected virtual void HandleSecondStage()
    {
        if (!bossEnemy.HasSecondStage || inSecondStage) return;

        float healthRatio = bossEnemy.GetHealth() / bossEnemy.MaxHealth;
        if (healthRatio <= bossEnemy.SecondStageHealthThreshold)
        {
            inSecondStage = true;
            OnEnterSecondStage();
        }
    }

    // Can be called to deal damage to the target if they are a player
    protected virtual void DealDamage(float amount)
    {
        if (target != null && target.TryGetComponent(out PlayerLogic _))
        {
            PlayerLogic.Instance.TakeDamage(amount);
        }
    }

    // To be overriden in child boss classes
    protected abstract void PrimaryAttack();
    protected abstract void SecondaryAttack();
    protected virtual void OnEnterSecondStage()
    {
        Debug.Log("Second stage has been entered!");
    }
}
