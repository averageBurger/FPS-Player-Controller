using System;
using System.Collections;
using UnityEngine;

public abstract class MeleeWeapon : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected int damage;
    [SerializeField] protected float cooldown;
    [SerializeField] protected float weaponReach;

    protected float attackShapeWidth;
    protected float attackShapeHeight;

    public event EventHandler OnMeleeWeaponAttack;

    private bool canAttack = true;

    public void Attack(Vector3 direction) {
        if (!canAttack) {
            return;
        }
        Debug.Log("can attack.");

        if (cooldown != 0) {
            StartCoroutine(StartAttackCooldown());
        }
        OnMeleeWeaponAttack?.Invoke(this, EventArgs.Empty);

        Vector3 spawn = WeaponManager.Instance.GetMeleeAttackShapeSpawnPoint();
        Vector3 halfExtents = new Vector3(attackShapeWidth / 2f, attackShapeHeight / 2f, 0.05f);
        RaycastHit[] raycastHitArray = Physics.BoxCastAll(spawn, halfExtents, direction, Quaternion.identity, weaponReach, WeaponManager.Instance.GetWeaponInteractableLayerMask());

        foreach (RaycastHit hit in raycastHitArray) {
            Debug.Log(hit.transform.gameObject.name);
            int layer = hit.transform.gameObject.layer;
            if (WeaponManager.Instance.HasHitEnemy(layer) && !IsBehindObstacle(hit)) {
                Debug.Log("hit enemy!");
                HitEnemyEffect(hit.transform.gameObject);
            } else if (WeaponManager.Instance.HasHitObstacle(layer)) {
                Debug.Log("hit obstacle!");
                HitObstacleEffect(hit.transform.gameObject);
            }
        }

        return;
    }

    private bool IsBehindObstacle(RaycastHit r) {
        Vector3 spawn = WeaponManager.Instance.GetMeleeAttackShapeSpawnPoint();
        Vector3 dir = (r.transform.position - spawn).normalized;
        Physics.Raycast(spawn, dir, out RaycastHit raycastHit, weaponReach * 10, WeaponManager.Instance.GetWeaponInteractableLayerMask());
        int layer = raycastHit.transform.gameObject.layer;

        return !WeaponManager.Instance.HasHitEnemy(layer);
    }

    private IEnumerator StartAttackCooldown() {
        canAttack = false;
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }

    protected abstract void HitEnemyEffect(GameObject gameObjectHit);
    protected virtual void HitObstacleEffect(GameObject GameObjecthit) { }
}
