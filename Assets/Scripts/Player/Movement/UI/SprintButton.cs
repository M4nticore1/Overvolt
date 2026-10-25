using UnityEngine;
using UnityEngine.UI;

public class SprintButton : MonoBehaviour
{
    private static SprintButton instance;
    public static SprintButton Instance => instance ??= FindFirstObjectByType<SprintButton>();

    private PlayerMoveController MoveController => PlayerMoveController.Instance;
    private PlayerSprintController SprintController => PlayerSprintController.Instance;

    [SerializeField] private Button button;
    public Button Button => button;

    private void Awake()
    {
        if (instance != null && instance != this) {
            Debug.LogError("Instance is not null!");
            Destroy(this);
            return;
        }

        instance = this;
    }

    private void OnEnable()
    {
        button.onClick.AddListener(HandleButtonClicked);
        MoveController.OnSprintActiveChanged += HandleSprintActiveChanged;
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(HandleButtonClicked);
        MoveController.OnSprintActiveChanged -= HandleSprintActiveChanged;
    }
    
    private void HandleButtonClicked()
    {
        SprintController.ActiveSprint();
    }

    private void HandleSprintActiveChanged(bool active)
    {
        button.interactable = !active;
    }
}