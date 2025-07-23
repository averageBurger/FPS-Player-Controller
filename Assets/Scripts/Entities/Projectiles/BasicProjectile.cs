using UnityEngine;

public class BasicProjectile : EnemyProjectile
{
    // Unique to the projectile
    [Header("Projectile Stats")]
    [SerializeField] public override float ProjectileDamage => 40f;
    [SerializeField] public override float ProjectileSpeed => 30f;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.root == target) 
        {
            // Check if the target hit is assigned the PlayerLogicManager...
            // ...if so, they must be a player so we can deal damage to them
            // If we don't do this and the target is not a player, the player will still take damage
            PlayerLogic player = PlayerLogic.Instance;

            player.TakeDamage(ProjectileDamage);
                
            // TO TEST STATUS EFFECTS
            // REMOVE/ALTER LATER ON!!!
            // player.ApplyStatusEffect(new BurnEffect(10f));
            // var effects = player.GetActiveStatusEffects();
            // Debug.Log($"Set player on fire: Active effects {StatusEffectUtils.FormatActiveStatusEffects(effects)}");
        }

        Destroy(gameObject);
    }
}