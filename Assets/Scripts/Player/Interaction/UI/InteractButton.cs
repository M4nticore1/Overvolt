using UnityEngine;
using UnityEngine.UI;

public class InteractButton : MonoBehaviour
{
    private static InteractButton instance;
    public static InteractButton Instance => instance ??= FindFirstObjectByType<InteractButton>();

    private PlayerInteractionController InteractionController => PlayerInteractionController.Instance;

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
        InteractionController.OnTargetSet += HandleTargetSet;
        InteractionController.OnTargetRemoved += HandleTargetRemove;

        button.onClick.AddListener(HandleButtonClicked);
    }

    private void OnDisable()
    {
        InteractionController.OnTargetSet -= HandleTargetSet;
        InteractionController.OnTargetRemoved -= HandleTargetRemove;

        button.onClick.RemoveListener(HandleButtonClicked);
    }

    private void Start()
    {
        UpdateInteractable();
    }

    private void UpdateInteractable()
    {
        button.interactable = InteractionController.CurrentTarget != null;
    }

    private void HandleTargetSet(IPlayerInteractionTarget target)
    {
        UpdateInteractable();
    }

    private void HandleTargetRemove(IPlayerInteractionTarget target)
    {
        UpdateInteractable();
    }

    private void HandleButtonClicked()
    {
        InteractionController.Interact();
    }
}