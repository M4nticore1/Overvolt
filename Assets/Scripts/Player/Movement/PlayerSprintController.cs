using UnityEngine;

public class PlayerSprintController : MonoBehaviour
{
    public static PlayerSprintController instance;
    public static PlayerSprintController Instance => instance ??= FindFirstObjectByType<PlayerSprintController>();

    [Header("Main")]
    [SerializeField] private PlayerMoveController moveController;

    [Header("Params")]
    [SerializeField] private float sprintDuration = 2f;
    private float currentSprintDuration = 0f;
    private float currentSprintTime = 0f;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (moveController.isSprinting) {
            currentSprintTime += Time.deltaTime;

            if (currentSprintTime >= currentSprintDuration) {
                DeactiveSprint();
            }
        }
    }

    public void ActiveSprint(float duration = 0f)
    {
        if (moveController.SetSprintActive(true)) {
            currentSprintDuration = duration > 0f ? duration : sprintDuration;
            currentSprintTime = 0f;
        }
    }

    public void DeactiveSprint()
    {
        moveController.SetSprintActive(false);
        currentSprintTime = 0f;
    }
}