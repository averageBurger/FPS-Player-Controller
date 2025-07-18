using UnityEngine;

public class BasicProjectile : EnemyProjectile
{
    // Unique to the projectile
    [Header("Projectile Stats")]
    public override float ProjectileDamage => 25f;
    public override float ProjectileSpeed => 15f;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.root == target) 
        {
            // Check if the target hit is assigned the PlayerLogicManager...
            // ...if so, they must be a player so we can deal damage to them
            // If we don't do this and the target is not a player, the player will still take damage
            if (target.TryGetComponent(out PlayerLogicManager _))
            {
                PlayerLogicManager.Instance.TakeDamage(ProjectileDamage);
            }
        }

        Destroy(gameObject);
    }
}