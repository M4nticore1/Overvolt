using System;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    private static PlayerInteractionController instance;
    public static PlayerInteractionController Instance => instance ??= FindFirstObjectByType<PlayerInteractionController>();

    private MonoBehaviour currentTargetMono;

    public IPlayerInteractionTarget CurrentTarget => currentTargetMono != null ? currentTargetMono as IPlayerInteractionTarget : null;

    public event Action<IPlayerInteractionTarget> OnTargetSet;
    public event Action<IPlayerInteractionTarget> OnTargetRemoved;

    private void Awake()
    {
        if (instance != null && instance != this) {
            Debug.LogError("Instance is not null!");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void SetTarget(IPlayerInteractionTarget target)
    {
        if (target == null) {
            Debug.LogError("Target is not valid!");
            return;
        }

        var targetMono = target as MonoBehaviour;

        if (targetMono == currentTargetMono) return;

        if (CurrentTarget != null) {
            RemoveTarget(CurrentTarget);
        }

        currentTargetMono = targetMono;
        target.SetInteractTargetSelected(true);

        OnTargetSet?.Invoke(target);
    }

    public void RemoveTarget(IPlayerInteractionTarget target)
    {
        if (target == null) {
            Debug.LogError("Target is not valid!");
            return;
        }

        if (target != CurrentTarget) return;

        currentTargetMono = null;
        target.SetInteractTargetSelected(false);

        OnTargetRemoved?.Invoke(target);
    }

    public void Interact()
    {
        if (CurrentTarget == null) return;

        CurrentTarget.Interact();
    }
}