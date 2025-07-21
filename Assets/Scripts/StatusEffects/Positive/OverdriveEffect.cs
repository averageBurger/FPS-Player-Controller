using UnityEngine;

// Doubles the player's stamina when applied
public class OverdriveEffect : StatusEffect
{
    private float newStamina;

    public OverdriveEffect() : base(0f) // Immediately removes the effect after applying as it has no tick functionality
    {
    }

    public override void Apply(StatusEffectHandler handler)
    {
        base.Apply(handler);

        // Check if this effect has been applied on a player or entity
        if (PlayerLogicManager.Instance == null) { return; }

        var player = PlayerLogicManager.Instance;

        float currentStamina = player.GetCurrentStamina();

        // Gain the current stamina + current stamina (again) 
        // This doubles it as we gain the stamina not set the stamina
        newStamina = currentStamina;
        player.GainStamina(newStamina);
    }

    public override void Remove(StatusEffectHandler handler)
    {
        // Do nothing as this effect only does something upon being applied
    }
}
