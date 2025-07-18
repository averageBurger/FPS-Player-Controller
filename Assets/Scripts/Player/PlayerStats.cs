using Mono.Cecil.Cil;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    private PlayerLogicManager playerLogicManager;
    public Image HealthBar;
    public Image StaminaBar;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerLogicManager = player.GetComponent<PlayerLogicManager>();
    }

    private void FixedUpdate()
    {
        if (playerLogicManager != null)
        {
            DamageChecker(playerLogicManager.GetCurrentHealth(), playerLogicManager.GetMaxHealth());
            StaminaChecker(playerLogicManager.GetCurrentStamina(), playerLogicManager.GetMaxStamina());
        }
    }

    private void DamageChecker(float health, float maxHealth)
    {
        HealthBar.fillAmount = health / maxHealth;
    }

    private void StaminaChecker(float stamina, float staminaMax)
    {
        StaminaBar.fillAmount = stamina / staminaMax;
    }
}
