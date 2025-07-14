using System.Collections;
using UnityEngine;

public class HitscanWeaponVisuals : MonoBehaviour
{
    [SerializeField] private RangedWeapon weapon;
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
        transform.Rotate(Vector3.forward * 70);
        yield return new WaitForSeconds(weapon.GetReloadTime());
        transform.Rotate(Vector3.back * 70);
    }

    private void Weapon_OnRangedWeaponShot(object sender, System.EventArgs e) {
        Vector3 spawn = visualBulletSpawnPoint.position;

        lineRenderer.SetPositions(new Vector3[2] {
            spawn,
            new Vector3 (spawn.x + 2, spawn.y, spawn.z)
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
