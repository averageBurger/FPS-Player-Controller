using UnityEngine;

public abstract class StatusEffect
{
    public float duration;
    protected float timeElapsed;

    private float intervalTimer = 1f; // Every second

    public bool IsFinished => timeElapsed >= duration;
    public float RemainingTime() => Mathf.Max(0f, duration - timeElapsed);

    public StatusEffect(float duration)
    {
        this.duration = duration;
    }

    // Calculates when a second has occurred
    public virtual void CalculateInterval(float deltaTime)
    {
        duration -= deltaTime;

        intervalTimer -= deltaTime;
        if (intervalTimer <= 0f) 
        {
            intervalTimer = 1f;
            OnInterval();
        }
    }

    // Called every second
    protected virtual void OnInterval() { }

    // Used to refresh an effect if applied again whilst it is already active
    public virtual void Refresh(float newDuration)
    {
        duration = newDuration;
        timeElapsed = 0f;
    }


    public virtual void Apply(StatusEffectHandler handler) { }
    public virtual void Remove(StatusEffectHandler handler) { }
}
