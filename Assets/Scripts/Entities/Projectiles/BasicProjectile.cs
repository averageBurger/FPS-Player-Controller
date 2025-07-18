using UnityEngine;

public class BasicProjectile : EnemyProjectile
{
    // Unique to the projectile
    [Header("Projectile Stats")]
    public override float ProjectileDamage => 25f;
    public override float ProjectileSpeed => 15f;

    public Transform Player;

    private void OnCollisionEnter(Collision collision)
    {
        // The projectile must be assigned the player object
        // I'm not a fan of the way I have implemented this but it works for now...
        if(collision.transform.root == Player) 
        {
            PlayerLogicManager.Instance.TakeDamage(ProjectileDamage);
        }

        Destroy(gameObject);
    }
}