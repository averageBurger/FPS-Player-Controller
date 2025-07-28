using UnityEngine;

// Contains the base enemy AI related functions for children to inherit
// All of the enemy's AI share functions from here unless specified otherwise
// When creating AI for an enemy, it should be a child of this class!
public class EnemyAI : MonoBehaviour
{
    public Transform target;
    protected StatusEffectHandler effectHandler;
    private Entity entity;

    protected void Start()
    {
        effectHandler = GetComponent<StatusEffectHandler>();
        entity = GetComponent<Entity>();
    }

    protected void Update()
    {
        if (effectHandler != null && effectHandler.HasEffect<StunEffect>()) { return; }
    }

    // Cast a ray from the enemy's origin point defined to be their eye level, if the ray hits the player they have been seen -> return true
    public bool CanSeeTarget()
    {
        // If the enemy is blinded they do not see anything (obviously)
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
        bool isBlinded = effectHandler != null && effectHandler.HasEffect<BlindEffect>();
        bool looksAtTarget = entity != null && entity.GetLooksAtTarget();

        // If the enemy is blind/does not look at the target/the target is null we do not look in their direction
        if (isBlinded || !looksAtTarget || target == null) { return; }

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