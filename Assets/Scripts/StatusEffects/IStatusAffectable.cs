using UnityEngine;

// Interface for all entities and player(s) allowing them to have status effects

public interface IStatusAffectable
{
    void ApplyStatusEffect(StatusEffect effect);
    bool HasEffect<E>() where E : StatusEffect;
    void TakeDamage(float damage);
}
