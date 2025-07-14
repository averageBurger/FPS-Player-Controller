using UnityEngine;
using UnityEngine.AI;

// Contains code for the behaviour of a ranged enemy
public class RangedEnemyAI : EnemyAI
{
    [Header("AI")]
    public float detectionRange = 15f;

    private NavMeshAgent navAgent;
    private Enemy enemyStats;
    private RangedEnemy rangedEnemyStats;

    private float attackCooldownTimer = 0f;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        enemyStats = GetComponent<Enemy>();
        rangedEnemyStats = GetComponent<RangedEnemy>();

        navAgent.speed = enemyStats.MovementSpeed;
    }

    void Update()
    {
        // Failsafe incase the player does not exist
        if (player == null)
        {
            Debug.LogWarning("Target player has not been assigned to an enemy!");
            return;
        }

        bool canSeePlayer = CanSeePlayer();

        if (canSeePlayer)
        {
            // FireProjectile();
            attackCooldownTimer = enemyStats.AttackCooldown;
        }

        // TODO: Move enemy to a better position

        if (attackCooldownTimer > 0f)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
    }
}