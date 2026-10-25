using UnityEngine;

public class BuildingPlaceMenu : MonoBehaviour
{
    private static BuildingPlaceMenu instance;
    public static BuildingPlaceMenu Instance => instance ??= FindFirstObjectByType<BuildingPlaceMenu>();

    [SerializeField] private AnimatedPanel animatedPanel;

    public bool IsShown => animatedPanel.IsShown;

    private void Awake()
    {
        if (instance != null && instance != this) {
            Debug.LogError("Insntance is not null!");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void Show(BuildingPlace buildingPlace)
    {
        animatedPanel.Show();
    }

    public void Hide()
    {
        animatedPanel.Hide();
    }
}