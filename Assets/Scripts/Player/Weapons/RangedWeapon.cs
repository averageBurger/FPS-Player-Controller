using UnityEngine;

// CREATED BY: Sean
// abstract class to act as a parent to all ranged weapons.
public abstract class RangedWeapon : MonoBehaviour
{
    public abstract void Shoot(Vector3 startPos, Vector3 direction);
    public virtual void Reload() {

    }
}
