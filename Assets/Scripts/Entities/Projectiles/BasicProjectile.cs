using UnityEngine;

public class BasicProjectile : EnemyProjectile
{
    // Unique to the projectile
    [Header("Projectile Stats")]
    [SerializeField] public override float ProjectileDamage => 30f;
    [SerializeField] public override float ProjectileSpeed => 30f;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform == target) 
        {
            if (target.TryGetComponent(out PlayerLogic _))
            {
                PlayerLogic.Instance.TakeDamage(ProjectileDamage);
            }

            // THE CODE BELOW IS TO TEST STATUS EFFECTS
            // REMOVE/ALTER LATER ON!!!
            // player.ApplyStatusEffect(new BurnEffect(10f));
            // var effects = player.GetActiveStatusEffects();
            // Debug.Log($"Set player on fire: Active effects {StatusEffectUtils.FormatActiveStatusEffects(effects)}");
        }

        Destroy(gameObject);
    }
}