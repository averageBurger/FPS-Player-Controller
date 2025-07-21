using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private PlayerLogic playerLogicManager;

    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Image HealthBar;
    [SerializeField] private Image StaminaBar;
    [SerializeField] private GameObject deathTextGO;
    
    private void Start() {
        playerLogicManager = PlayerLogic.Instance;

        WeaponManager_OnAmmoChanged(this, EventArgs.Empty);
        WeaponManager.Instance.OnAmmoChanged += WeaponManager_OnAmmoChanged;
        playerLogicManager.OnHealthChanged += PlayerLogicManager_OnHealthChanged;
        playerLogicManager.OnStaminaChanged += PlayerLogicManager_OnStaminaChanged;
        playerLogicManager.OnDeathStatusChanged += PlayerLogicManager_OnDeathStatusChanged;

        deathTextGO.SetActive(false);
    }

    private void WeaponManager_OnAmmoChanged(object sender, EventArgs e) {
        RangedWeapon weapon = WeaponManager.Instance.GetEquippedRangedWeapon();
        int magazineAmmo = weapon.GetAmmoInMagazine();
        int reserveAmmo = weapon.GetAmmoInReserve();

        ammoText.text = magazineAmmo.ToString() + " | " + reserveAmmo.ToString();
    }

    private void PlayerLogicManager_OnHealthChanged(object sender, EventArgs e) {
        DamageChecker(playerLogicManager.GetCurrentHealth(), playerLogicManager.GetMaxHealth());
    }

    private void PlayerLogicManager_OnStaminaChanged(object sender, EventArgs e) {
        StaminaChecker(playerLogicManager.GetCurrentStamina(), playerLogicManager.GetMaxStamina());
    }

    private void PlayerLogicManager_OnDeathStatusChanged(object sender, EventArgs e) {
        deathTextGO.SetActive(playerLogicManager.IsDead());
    }

    private void DamageChecker(float health, float maxHealth) {
        HealthBar.fillAmount = health / maxHealth;
    }

    private void StaminaChecker(float stamina, float staminaMax) {
        StaminaBar.fillAmount = stamina / staminaMax;
    }

    private void OnDestroy() {
        WeaponManager.Instance.OnAmmoChanged -= WeaponManager_OnAmmoChanged;
        playerLogicManager.OnHealthChanged -= PlayerLogicManager_OnHealthChanged;
        playerLogicManager.OnStaminaChanged -= PlayerLogicManager_OnStaminaChanged;
    }
}
