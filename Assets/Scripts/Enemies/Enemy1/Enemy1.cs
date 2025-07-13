using Unity.VisualScripting;
using UnityEngine;

// Stats for Enemy1 which we inherit from the Enemy base class
// We can override stats which need to be changed for Enemy1 here
// By default, all enemies would use the stats of the Enemy base class unless specified otherwise
public class Enemy1 : Enemy
{
    // Modify the base enemy attack damage to fit Enemy1's attack damage
    public override float GetAttackDamage()
    {
        return base.attackDamage = 25f;
    }
}
