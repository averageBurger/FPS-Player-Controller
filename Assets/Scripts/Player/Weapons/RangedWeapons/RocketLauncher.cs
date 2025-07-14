using UnityEngine;

public class RocketLauncher : RangedWeapon
{
    public override bool Shoot(Vector3 startPos, Vector3 direction) {
        if (!base.Shoot(startPos, direction)) {
            return false;
        }



        return true;
    }
}
