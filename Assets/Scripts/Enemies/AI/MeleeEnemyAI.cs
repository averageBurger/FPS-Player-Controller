using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyAI : MonoBehaviour
{
    public Transform player;

    private NavMeshAgent navAgent;
    private Enemy enemyStats;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        enemyStats = GetComponent<Enemy>();
    }

    void Update()
    {
        // Check if the player is within the enemy's FOV
        // Check if the player is within the enemy's attack range 
    }

    private void FollowPlayer()
    {
        // Pathfind to the player using the specified NavMesh for the level
        // Stop following when close enough to the player to be able to attack
    }

    private void AttackPlayer()
    {
        // Look at the player
        // Attack using respective weapon
        // Initiate cooldown

    }


}
