using System;
using UnityEngine;

public class PlayerShootTargetController : MonoBehaviour
{
    private static PlayerShootTargetController instance;
    public static PlayerShootTargetController Instance => instance ??= FindFirstObjectByType<PlayerShootTargetController>();

    public IPlayerCombatTarget CurrentTarget { get; private set; }

    public event Action<IPlayerCombatTarget> OnTargetSet;
    public event Action<IPlayerCombatTarget> OnTargetRemoved;

    private void Awake()
    {
        if (instance != null && instance != this) {
            Debug.LogError("Instance is not valid!");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void OnDestroy()
    {
        if (CurrentTarget != null && CurrentTarget.HealthComponent) {
            CurrentTarget.HealthComponent.OnDied -= HandleTargetDied;
        }
    }

    public void SetTarget(IPlayerCombatTarget target)
    {
        if (target == null) {
            Debug.LogError("Target is not valid!");
            return;
        }

        if (target == CurrentTarget) return;
        if (target.HealthComponent != null && !target.HealthComponent.IsAlive) return;

        if (CurrentTarget != null) {
            RemoveTarget(CurrentTarget);
        }

        CurrentTarget = target;

        if (CurrentTarget.HealthComponent != null)
            CurrentTarget.HealthComponent.OnDied += HandleTargetDied;

        OnTargetSet?.Invoke(target);
    }

    public void RemoveTarget(IPlayerCombatTarget target)
    {
        if (target == null) {
            Debug.LogError("Target is not valid!");
            return;
        }

        if (target != CurrentTarget) return;

        CurrentTarget = null;

        if (target.HealthComponent != null)
            target.HealthComponent.OnDied -= HandleTargetDied;

        OnTargetRemoved?.Invoke(target);
    }

    private void HandleTargetDied()
    {
        RemoveTarget(CurrentTarget);
    }
}