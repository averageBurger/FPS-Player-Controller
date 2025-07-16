using TMPro;
using System;
using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;

    private void Start() {
        WeaponManager_OnAmmoChanged(this, EventArgs.Empty);
        WeaponManager.Instance.OnAmmoChanged += WeaponManager_OnAmmoChanged;
    }

    private void WeaponManager_OnAmmoChanged(object sender, EventArgs e) {
        RangedWeapon weapon = WeaponManager.Instance.GetEquippedRangedWeapon();
        int magazineAmmo = weapon.GetAmmoInMagazine();
        int reserveAmmo = weapon.GetAmmoInReserve();

        ammoText.text = magazineAmmo.ToString() + " | " + reserveAmmo.ToString();
    }

    private void OnDestroy() {
        WeaponManager.Instance.OnAmmoChanged -= WeaponManager_OnAmmoChanged;
    }
}
