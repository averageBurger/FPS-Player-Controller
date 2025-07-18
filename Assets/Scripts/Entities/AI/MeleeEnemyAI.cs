using UnityEngine;
using UnityEngine.AI;

// Contains code for the behaviour and pathfinding of a melee enemy
public class MeleeEnemyAI : EnemyAI
{
    private NavMeshAgent navAgent;
    private MeleeEnemy enemy;

    private float attackCooldownTimer = 0f;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<MeleeEnemy>();

        navAgent.speed = enemy.MovementSpeed;
    }

    void Update()
    {
        // Failsafe incase the player does not exist
        if (player == null)
        {
            Debug.LogWarning("Target player has not been assigned to an enemy!");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool canSeePlayer = CanSeePlayer();

        if (distanceToPlayer <= enemy.AttackRange && canSeePlayer) 
        { 
            AttackPlayer();
        }
        else if (distanceToPlayer <= DetectionRange && canSeePlayer)
        {
            FollowPlayer();
        }
        else
        {
            // If the player is not in range/not visible we stop moving
            navAgent.isStopped = true;
        }

        if (attackCooldownTimer > 0f)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
    }

    // Uses the NavMesh to navigate to the player
    private void FollowPlayer()
    {
        LookAtPlayer();

        navAgent.isStopped = false;
        navAgent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        navAgent.isStopped = true;

        LookAtPlayer();

        if (attackCooldownTimer <= 0f) 
        {
            PlayerLogicManager.Instance.TakeDamage(enemy.AttackDamage);
            // TODO: Trigger animation

            attackCooldownTimer = enemy.AttackCooldown;
        }
    }
}