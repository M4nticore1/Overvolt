using System;
using UnityEngine;

public class PlayerShootingController : MonoBehaviour
{
    private static PlayerShootingController instance;
    public static PlayerShootingController Instance => instance ??= FindFirstObjectByType<PlayerShootingController>();

    public IPlayerTarget CurrentTarget { get; private set; }

    public event Action<IPlayerTarget> OnTargetSet;
    public event Action<IPlayerTarget> OnTargetRemoved;

    private void Awake()
    {
        instance = this;
    }

    public void SetTarget(IPlayerTarget target)
    {
        if (target == null) {
            Debug.LogError("Target is not valid!");
            return;
        }

        if (target == CurrentTarget) return;

        if (CurrentTarget != null) {
            RemoveTarget(CurrentTarget);
        }

        CurrentTarget = target;

        OnTargetSet?.Invoke(target);
    }

    public void RemoveTarget(IPlayerTarget target)
    {
        if (target == null) {
            Debug.LogError("Target is not valid!");
            return;
        }

        if (target != CurrentTarget) return;

        CurrentTarget = null;

        OnTargetRemoved?.Invoke(target);
    }
}