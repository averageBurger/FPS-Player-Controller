using UnityEngine;
using UnityEngine.AI;

// Contains code for the behaviour and pathfinding of a melee enemy
public class MeleeEnemyAI : EnemyAI
{
    private NavMeshAgent navAgent;
    private MeleeEnemy enemy;

    private float attackCooldownTimer = 0f;

    new void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<MeleeEnemy>();

        navAgent.speed = enemy.GetMovementSpeed();
    }

    new void Update()
    {
        // Failsafe incase the target does not exist
        if (target == null)
        {
            Debug.LogWarning("A target has not been assigned to a melee enemy!");
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        bool canSeeTarget = CanSeeTarget();

        if (distanceToTarget <= enemy.GetAttackRange() && canSeeTarget) 
        { 
            AttackTarget();
        }
        else if (distanceToTarget <= GetDetectionRange() && canSeeTarget)
        {
            FollowTarget();
        }
        else
        {
            // If the target is not in range/not visible we stop moving
            navAgent.isStopped = true;
        }

        if (attackCooldownTimer > 0f)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
    }

    // Uses the NavMesh to navigate to the target
    private void FollowTarget()
    {
        LookAtTarget();

        navAgent.isStopped = false;
        navAgent.SetDestination(target.position);
    }

    private void AttackTarget()
    {
        navAgent.isStopped = true;

        LookAtTarget();

        if (attackCooldownTimer <= 0f) 
        {
            // Check if the target hit is assigned the PlayerLogicManager...
            // ...if so, they must be a player so we can deal damage to them
            // If we don't do this and the target is not a player, the player will still take damage
            if (target.TryGetComponent(out PlayerLogicManager _))
            {
                PlayerLogicManager.Instance.TakeDamage(enemy.GetAttackDamage());
            }

            attackCooldownTimer = enemy.GetAttackCooldown(); 
        }
    }
}