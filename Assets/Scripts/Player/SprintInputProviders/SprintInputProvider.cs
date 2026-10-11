using UnityEngine;

public abstract class SprintInputProvider : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private PlayerSprintController sprintController;

    protected void SetSprintActive(bool active)
    {
        if (active) {
            sprintController.ActiveSprint();
        }
        else {
            sprintController.DeactiveSprint();
        }
    }
}