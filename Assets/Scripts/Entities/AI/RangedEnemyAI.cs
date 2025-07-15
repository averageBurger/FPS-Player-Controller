using System.IO.Pipes;
using UnityEngine;
using UnityEngine.AI;

// Contains code for the behaviour of a ranged enemy
public class RangedEnemyAI : EnemyAI
{
    private NavMeshAgent navAgent;
    private RangedEnemy enemy;

    private float attackCooldownTimer = 0f;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<RangedEnemy>();

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

        bool canSeePlayer = CanSeePlayer();

        if (canSeePlayer && attackCooldownTimer <= 0f)
        {
            FireProjectile();
            attackCooldownTimer = enemy.ReloadCooldown;
        }

        // TODO: Move enemy to a better position

        if (attackCooldownTimer > 0f)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
    }

    private void FireProjectile()
    {
        // Failsafe
        if (enemy.ProjectileObject == null || enemy.ProjectileFirePoint == null)
        {
            Debug.LogWarning("No projectile object or fire point has been assigned to a ranged enemy!");
            return;
        }

        Transform firePoint = enemy.ProjectileFirePoint;
        Vector3 fireDirection = firePoint.forward.normalized;
        float spawnOffsetDistance = 0.5f;

        Vector3 spawnPosition = firePoint.position + fireDirection * spawnOffsetDistance;
        Quaternion spawnRotation = Quaternion.LookRotation(fireDirection);

        GameObject projectileObject = Instantiate(enemy.ProjectileObject, spawnPosition, spawnRotation);

        IProjectile projectile = projectileObject.GetComponent<IProjectile>();

        if(projectile != null)
        {
            Vector3 direction = (player.position + Vector3.up - projectileObject.transform.position).normalized;
            projectile.Spawn(direction);
        }
        else
        {
            Debug.LogWarning("Spawned projectile does not implement the IProjectile interface!");
        }
    }
}