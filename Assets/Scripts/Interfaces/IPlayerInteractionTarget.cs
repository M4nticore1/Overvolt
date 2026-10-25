using System;
using UnityEngine;

public interface IPlayerInteractionTarget
{
    public GameObject MarkerPrefab { get; }
    public Transform InteractionMarkerPoint { get; }

    public event Action OnInteracted;
    public event Action<bool> OnSelectedChanged;

    public void SetInteractTargetSelected(bool selected);
    public void Interact();
}