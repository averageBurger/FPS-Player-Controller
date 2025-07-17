using UnityEngine;

public class BasicProjectile : EnemyProjectile
{
    // Unique to the projectile
    [Header("Projectile Stats")]
    public override float ProjectileDamage => 44f;
    public override float ProjectileSpeed => 15f;

    public Transform Player;

    private void OnCollisionEnter(Collision collision)
    {
        // Temporary solution until the player is coded
        // This is not a good way of doing this >:(
        if(collision.transform.root == Player) 
        {
            Debug.Log($"Enemy attacks for {ProjectileDamage} damage!");
        }

        Destroy(gameObject);
    }
}