using UnityEngine;
using UnityEngine.UI;

public class SprintButton : MonoBehaviour
{
    private PlayerMoveController MoveController => PlayerMoveController.Instance;
    private PlayerSprintController SprintController => PlayerSprintController.Instance;

    [SerializeField] private Button button;

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