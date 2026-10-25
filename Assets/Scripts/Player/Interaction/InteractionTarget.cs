using System;
using UnityEngine;

public class InteractionTarget : MonoBehaviour, IPlayerInteractionTarget
{
    [SerializeField] private GameObject markerPrefab;
    public GameObject MarkerPrefab => markerPrefab;

    [SerializeField] private Transform interactionMarkerPoint;
    public Transform InteractionMarkerPoint => interactionMarkerPoint;

    public event Action OnInteracted;
    public event Action<bool> OnSelectedChanged;

    public void SetInteractTargetSelected(bool selected)
    {
        OnSelectedChanged?.Invoke(selected);
    }

    public void Interact()
    {
        OnInteracted?.Invoke();
    }
}