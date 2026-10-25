using UnityEngine;

[RequireComponent(typeof(PlayerInteractionController))]
public class PlayerInteractionMarkerController : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private PlayerInteractionController interactionTargetController;

    [Header("Params")]
    private GameObject spawnedMarker;

    private void OnEnable()
    {
        interactionTargetController.OnTargetSet += HandleTargetSet;
        interactionTargetController.OnTargetRemoved += HandleTargetRemoved;
    }

    private void OnDisable()
    {
        interactionTargetController.OnTargetSet -= HandleTargetSet;
        interactionTargetController.OnTargetRemoved -= HandleTargetRemoved;
    }

    private void SpawnMarker(Transform transform)
    {
        spawnedMarker = Instantiate(interactionTargetController.CurrentTarget.MarkerPrefab, transform);
    }

    private void DestroyMarker()
    {
        if (spawnedMarker != null) {
            Destroy(spawnedMarker.gameObject);
        }
    }

    private void HandleTargetSet(IPlayerInteractionTarget target)
    {
        DestroyMarker();
        SpawnMarker(target.InteractionMarkerPoint);
    }

    private void HandleTargetRemoved(IPlayerInteractionTarget target)
    {
        DestroyMarker();
    }
}