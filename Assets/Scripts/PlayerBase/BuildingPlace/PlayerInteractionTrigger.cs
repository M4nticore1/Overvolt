using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayerInteractionTrigger : MonoBehaviour
{
    [SerializeField] private MonoBehaviour playerInteractionTarget;
    private IPlayerInteractionTarget PlayerInteractionTarget => playerInteractionTarget as IPlayerInteractionTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        var interactTargetController = other.GetComponent<PlayerInteractionController>();
        if (interactTargetController == null) return;

        interactTargetController.SetTarget(PlayerInteractionTarget);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == null) return;

        var interactTargetController = other.GetComponent<PlayerInteractionController>();
        if (interactTargetController == null) return;

        interactTargetController.RemoveTarget(PlayerInteractionTarget);
    }
}