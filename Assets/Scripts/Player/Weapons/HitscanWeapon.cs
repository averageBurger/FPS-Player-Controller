using UnityEngine;

public abstract class HitscanWeapon : RangedWeapon
{
    public override bool Shoot(Vector3 direction) {
        if (!base.Shoot(direction)) {
            return false;
        }

        if (!Physics.Raycast(WeaponManager.Instance.GetBulletSpawnPoint(), direction, out RaycastHit raycastHit, range, WeaponManager.Instance.GetWeaponInteractableLayerMask())) {
            return true;
        }
        int layer = raycastHit.transform.gameObject.layer;
        if (WeaponManager.Instance.HasHitObstacle(layer)) {
            HitObstacleEffect();
        } else if (WeaponManager.Instance.HasHitEnemy(layer)) {
            HitEnemyEffect(WeaponManager.Instance.HasHitEnemyWeakpoint(layer));
        }
        return true;
    }

    protected abstract void HitEnemyEffect(bool hitWeakpoint);
    protected virtual void HitObstacleEffect() { }
}
