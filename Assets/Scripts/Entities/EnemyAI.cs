using UnityEngine;

// Contains the base enemy AI related functions for children to inherit
// All of the enemy's AI share functions from here unless specified otherwise
// When creating AI for an enemy, it should be a child of this class!
public class EnemyAI : Entity
{
    public Transform target;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (effectHandler != null && effectHandler.HasEffect<StunEffect>()) { return; }
    }

    // Cast a ray from the enemy's origin point defined to be their eye level, if the ray hits the player they have been seen -> return true
    public bool CanSeeTarget()
    {
        if (effectHandler != null && effectHandler.HasEffect<BlindEffect>()) { return false;  }

        // Assumption that all enemies are of the same height, this is fine for now
        // Enemy's face is on the Z axis therefore we set the eyes on this axis
        Vector3 eyeOffset = transform.forward * 0.5f + Vector3.up * 1.5f;
        Vector3 originPoint = transform.position + eyeOffset;

        Vector3 direction = (target.position - originPoint).normalized;
        float distance = Vector3.Distance(originPoint, target.position);

        LayerMask layerMask = ~LayerMask.GetMask("Enemy"); // Invert the layer mask to only ignore the enemies layer (seeing through enemies)

        if (Physics.Raycast(originPoint, direction, out RaycastHit hit, distance, layerMask))
        {
            return hit.transform == target;
        }
        // Player is not in view
        return false;
    }

    public void LookAtTarget()
    {
        if (target == null) { return; }
        if (effectHandler != null && effectHandler.HasEffect<BlindEffect>() || !LooksAtTarget) { return; }

        // Aligns the enemy's Z axis towards the player
        // This makes the enemy object's Z axis the face of the enemy
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }
}