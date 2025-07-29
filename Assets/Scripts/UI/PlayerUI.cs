using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private PlayerLogic playerLogic;

    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Image healthBar;
    [SerializeField] private Image staminaBar;
    [SerializeField] private GameObject freeSlideIndicator;
    [SerializeField] private GameObject deathTextGO;
    
    private void Start() {
        playerLogic = PlayerLogic.Instance;

        WeaponManager_OnAmmoChanged(this, EventArgs.Empty);
        WeaponManager.Instance.OnAmmoChanged += WeaponManager_OnAmmoChanged;
        playerLogic.OnHealthChanged += PlayerLogic_OnHealthChanged;
        playerLogic.OnStaminaChanged += PlayerLogic_OnStaminaChanged;
        playerLogic.OnDeathStatusChanged += PlayerLogic_OnDeathStatusChanged;
        playerLogic.GetComponent<PlayerMovement>().OnFreeSlideThresholdPassed += PlayerMovement_OnFreeSlideThresholdPassed;

        deathTextGO.SetActive(false);
        freeSlideIndicator.SetActive(false);
    }

    private void WeaponManager_OnAmmoChanged(object sender, EventArgs e) {
        RangedWeapon weapon = WeaponManager.Instance.GetEquippedRangedWeapon();
        int magazineAmmo = weapon.GetAmmoInMagazine();
        int reserveAmmo = weapon.GetAmmoInReserve();

        ammoText.text = magazineAmmo.ToString() + " | " + reserveAmmo.ToString();
    }

    private void PlayerLogic_OnHealthChanged(object sender, EventArgs e) {
        DamageChecker(playerLogic.GetCurrentHealth(), playerLogic.GetMaxHealth());
    }

    private void PlayerLogic_OnStaminaChanged(object sender, EventArgs e) {
        StaminaChecker(playerLogic.GetCurrentStamina(), playerLogic.GetMaxStamina());
    }

    private void PlayerLogic_OnDeathStatusChanged(object sender, EventArgs e) {
        deathTextGO.SetActive(playerLogic.IsDead());
    }

    private void PlayerMovement_OnFreeSlideThresholdPassed(object sender, PlayerMovement.OnFreeSlideThresholdPassedEventArgs e) {
        freeSlideIndicator.SetActive(!e.shouldDrain);
    }

    private void DamageChecker(float health, float maxHealth) {
        healthBar.fillAmount = health / maxHealth;
    }

    private void StaminaChecker(float stamina, float staminaMax) {
        staminaBar.fillAmount = stamina / staminaMax;
    }

    private void OnDestroy() {
        WeaponManager.Instance.OnAmmoChanged -= WeaponManager_OnAmmoChanged;
        playerLogic.OnHealthChanged -= PlayerLogic_OnHealthChanged;
        playerLogic.OnStaminaChanged -= PlayerLogic_OnStaminaChanged;
        playerLogic.OnDeathStatusChanged -= PlayerLogic_OnDeathStatusChanged;
        //playerLogic.GetComponent<PlayerMovement>().OnFreeSlideThresholdPassed -= PlayerMovement_OnFreeSlideThresholdPassed;
    }
}
