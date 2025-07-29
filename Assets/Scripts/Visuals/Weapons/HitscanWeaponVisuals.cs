using System.Collections;
using UnityEngine;

public class HitscanWeaponVisuals : MonoBehaviour
{
    [SerializeField] private HitscanWeapon weapon;
    [SerializeField] private Transform visualBulletSpawnPoint;
    private LineRenderer lineRenderer;

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start() {
        weapon.OnRangedWeaponShot += Weapon_OnRangedWeaponShot;
        weapon.OnWeaponReload += Weapon_OnWeaponReload;
    }

    private void Weapon_OnWeaponReload(object sender, System.EventArgs e) {
        StartCoroutine(ReloadTempAnimation());
    }

    private IEnumerator ReloadTempAnimation() {
        transform.Rotate(Vector3.left * 70, Space.Self);
        yield return new WaitForSeconds(weapon.GetReloadTime());
        transform.Rotate(Vector3.right * 70, Space.Self);
    }

    private void Weapon_OnRangedWeaponShot(object sender, System.EventArgs e) {
        Vector3 spawn = visualBulletSpawnPoint.position;
        Vector3 inFrontOfSpawn = spawn + WeaponManager.Instance.GetLookingVector3() * 3;

        lineRenderer.SetPositions(new Vector3[2] {
            spawn,
            inFrontOfSpawn
        });
        lineRenderer.startWidth = 0.2f;
        StartCoroutine(ShotVisualTimerCoroutine());
    }

    private IEnumerator ShotVisualTimerCoroutine() {
        float shotFlashTime = 0.3f;
        yield return new WaitForSeconds(shotFlashTime);
        lineRenderer.startWidth = 0f;
    }

    private void OnDestroy() {
        weapon.OnRangedWeaponShot -= Weapon_OnRangedWeaponShot;
        weapon.OnWeaponReload -= Weapon_OnWeaponReload;
    }
}
