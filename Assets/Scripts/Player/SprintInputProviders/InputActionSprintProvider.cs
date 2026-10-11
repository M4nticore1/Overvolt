using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionSprintProvider : SprintInputProvider
{
    [Header("Input")]
    [SerializeField] private PlayerInputController playerInputController;

    private void OnEnable()
    {
        playerInputController.SprintIA.started += HandleSprintPressed;
    }

    private void OnDisable()
    {
        playerInputController.SprintIA.started -= HandleSprintPressed;
    }

    private void HandleSprintPressed(InputAction.CallbackContext ctx)
    {
        SetSprintActive(true);
    }
}