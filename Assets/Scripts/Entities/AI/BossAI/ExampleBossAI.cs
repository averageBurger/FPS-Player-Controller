using UnityEngine;

public class ExampleBossAI : BossEnemyAI
{
    // Only serialize fields that have been overriden
    // This makes sense because we shouldn't be serializing stats that aren't being changed...
    // ...from what is defined in the parent classes

    protected override void PrimaryAttack()
    {
        float damage = inSecondStage ? bossEnemy.GetPrimaryAttackDamage() * 2f : bossEnemy.GetPrimaryAttackDamage();
        DealDamage(damage);

        Debug.Log($"Primary Attack for {bossEnemy.GetPrimaryAttackDamage()} damage!");

        // TODO: Animation etc
    }

    protected override void SecondaryAttack()
    {
        float damage = inSecondStage ? bossEnemy.GetSecondaryAttackDamage() * 2f : bossEnemy.GetSecondaryAttackDamage();
        DealDamage(damage);

        Debug.Log($"Secondary Attack for {bossEnemy.GetSecondaryAttackDamage()} damage!");

        // TODO: Animation etc
    }

    protected override void OnEnterSecondStage()
    {
        base.OnEnterSecondStage();
    }
}
