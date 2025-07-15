using UnityEngine;

// Contains the base enemy AI related functions for children to inherit
// All of the enemy's AI share functions from here unless specified otherwise
// When creating AI for an enemy, it should be a child of this class!
public class EnemyAI : MonoBehaviour
{
    public virtual float DetectionRange { get; protected set; } = 15f;

    public Transform player;

    // Cast a ray from the enemy's origin point defined to be their eye level, if the ray hits the player they have been seen -> return true
    public bool CanSeePlayer()
    {
        Vector3 originPoint = transform.position + Vector3.up * 1.5f;
        // Assumption that all enemies are of the same height, this is fine for now
        Vector3 direction = (player.position - originPoint).normalized;
        LayerMask layerMask = ~LayerMask.GetMask("Enemies"); // Invert the layer mask to only ignore the enemies layer (seeing through enemies)
        float distance = Vector3.Distance(originPoint, player.position);

        if (Physics.Raycast(originPoint, direction, out RaycastHit hit, distance, layerMask))
        {
            return hit.transform == player;
        }
        // Player is not in view
        return false;
    }
}