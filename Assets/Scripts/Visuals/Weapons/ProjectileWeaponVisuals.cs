using UnityEngine;
using System.Collections;

public class ProjectileWeaponVisuals : MonoBehaviour
{
    [SerializeField] private ProjectileWeapon weapon;
    [SerializeField] private Transform visualEffectSpawnPoint;

    private void Start() {
        weapon.OnRangedWeaponShot += Weapon_OnRangedWeaponShot;
        weapon.OnWeaponReload += Weapon_OnWeaponReload;
    }

    private void Weapon_OnWeaponReload(object sender, System.EventArgs e) {
        StartCoroutine(ReloadTempAnimation());
    }

    private IEnumerator ReloadTempAnimation() {
        transform.Rotate(Vector3.left * 70);
        yield return new WaitForSeconds(weapon.GetReloadTime());
        transform.Rotate(Vector3.right * 70);
    }

    private void Weapon_OnRangedWeaponShot(object sender, System.EventArgs e) {
        Debug.Log("temp");
    }

    private void OnDestroy() {
        weapon.OnRangedWeaponShot -= Weapon_OnRangedWeaponShot;
        weapon.OnWeaponReload -= Weapon_OnWeaponReload;
    }
}
