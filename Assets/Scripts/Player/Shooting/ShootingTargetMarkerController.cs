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

    private void Awake()
    {
        if (shootingController == null)
            Debug.LogError("ShootingController is not valid!");
        if (markerPrefab == null)
            Debug.LogError("MarkerPrefab is not valid!");
    }

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
        if (spawnedMarker == null) return;

        Destroy(spawnedMarker.gameObject);
    }

    private void HandleTargetSet(IPlayerTarget target)
    {
        DestroyMarker();
        SpawnMarker(target.PlayerTargetMarkerTransform);
    }

    private void HandleTargetRemoved(IPlayerTarget target)
    {
        DestroyMarker();
    }
}