using UnityEngine;

public class RangedExample : RangedEnemy
{
    [SerializeField] private GameObject projectileObj;
    [SerializeField] private Transform projectileFirePoint;

    public override float MaxHealth => 80f;
    public override GameObject ProjectileObject => projectileObj;
    public override Transform ProjectileFirePoint => projectileFirePoint;
}
