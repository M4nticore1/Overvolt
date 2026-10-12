using System;
using UnityEngine;

[RequireComponent(typeof(PlayerShootingController))]
public class ShootingTargetMarkerController : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private PlayerShootingController shootingController;

    [Header("Params")]
    [SerializeField] private GameObject markerPrefab;
    private GameObject spawnedMarker;

    private void OnEnable()
    {
        shootingController.OnTargetSet += HandleTargetSet;
        shootingController.OnTargetRemoved += HandleTargetRemoved;
    }

    private void OnDisable()
    {
        shootingController.OnTargetSet -= HandleTargetSet;
        shootingController.OnTargetRemoved -= HandleTargetRemoved;
    }

    private void SpawnMarker(Transform transform)
    {
        spawnedMarker = Instantiate(markerPrefab, transform);
    }

    private void DestroyMarker()
    {
        if (spawnedMarker == null) {
            Debug.LogError("SpawnedMarker is not valid!");
            return;
        }

        Destroy(spawnedMarker.gameObject);
    }

    private void HandleTargetSet(IPlayerTarget target)
    {
        if (spawnedMarker != null) {
            DestroyMarker();
        }

        SpawnMarker(target.transform);
    }

    private void HandleTargetRemoved(IPlayerTarget target)
    {
        DestroyMarker();
    }
}