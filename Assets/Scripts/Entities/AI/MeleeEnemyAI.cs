using UnityEngine;
using UnityEngine.AI;

// Contains code for the behaviour and pathfinding of a melee enemy
public class MeleeEnemyAI : EnemyAI
{
    private NavMeshAgent navAgent;
    private MeleeEnemy enemy;
    private EnemyAI enemyAI;

    private float attackCooldownTimer = 0f;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<MeleeEnemy>();
        enemyAI = GetComponent<EnemyAI>();

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
        else if (distanceToPlayer <= enemyAI.DetectionRange && canSeePlayer)
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
        navAgent.isStopped = false;
        navAgent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        navAgent.isStopped = true;

        // TODO: Look at the player
        Vector3 lookingDir = player.position - transform.position;
        lookingDir.y = 0f;

        // Attack
        if (attackCooldownTimer <= 0f) 
        {
            Debug.Log($"Enemy attacks for {enemy.AttackDamage} damage!");
            // TODO: Trigger animation and deal damage to the player

            attackCooldownTimer = enemy.AttackCooldown;
        }
    }
}