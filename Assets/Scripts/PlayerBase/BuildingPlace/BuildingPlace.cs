using UnityEngine;

public class BuildingPlace : MonoBehaviour
{
    private BuildingPlaceMenu BuildingPlaceMenu => BuildingPlaceMenu.Instance;

    [SerializeField] private Building[] buildingPrefabs;
    [SerializeField] private Transform buildingPoint;

    [SerializeField] private InteractionTarget interactionTarget;

    private void OnEnable()
    {
        interactionTarget.OnSelectedChanged += HandleTargetSelectedChanged;
        interactionTarget.OnInteracted += HandleInteracted;
    }

    private void OnDisable()
    {
        interactionTarget.OnSelectedChanged -= HandleTargetSelectedChanged;
        interactionTarget.OnInteracted -= HandleInteracted;
    }

    private void HandleTargetSelectedChanged(bool selected)
    {
        if (!selected) {
            BuildingPlaceMenu.Hide();
        }
    }

    private void HandleInteracted()
    {
        if (BuildingPlaceMenu.IsShown) {
            BuildingPlaceMenu.Hide();
        }
        else {
            BuildingPlaceMenu.Show(this);
        }
    }
}