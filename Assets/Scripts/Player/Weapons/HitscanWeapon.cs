using UnityEngine;

public abstract class HitscanWeapon : RangedWeapon
{
    public override bool Shoot() {
        if (!base.Shoot()) {
            return false;
        }

        if (!Physics.Raycast(WeaponManager.Instance.GetBulletSpawnPoint(), WeaponManager.Instance.GetLookingVector3(), out RaycastHit raycastHit, range, WeaponManager.Instance.GetWeaponInteractableLayerMask())) {
            return true;
        }
        int layer = raycastHit.transform.gameObject.layer;
        if (WeaponManager.Instance.HasHitObstacle(layer)) {
            HitObstacleEffect(raycastHit.transform.gameObject);
        } else if (WeaponManager.Instance.HasHitEnemy(layer)) {
            HitEnemyEffect(raycastHit.transform.gameObject, false);
        }
        return true;
    }

    protected abstract void HitEnemyEffect(GameObject gameObjectHit, bool hitWeakpoint);
    protected virtual void HitObstacleEffect(GameObject gameObjectHit) { }
}
