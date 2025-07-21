using System.Collections.Generic;
using System.Text;

// Additional functions surrounding status effect functionality
public static class StatusEffectUtils
{
    // Formats the list of status effects a player/entity has to be more readable for UI use
    public static string FormatActiveStatusEffects(IReadOnlyList<StatusEffect> effects)
    {
        if (effects == null || effects.Count == 0)
            return "No active effects";

        StringBuilder sb = new StringBuilder();
        foreach (var effect in effects)
        {
            sb.Append(effect.GetType().Name);
            sb.Append(", ");
        }
        if (sb.Length > 2)
            sb.Length -= 2;

        return sb.ToString();
    }
}
