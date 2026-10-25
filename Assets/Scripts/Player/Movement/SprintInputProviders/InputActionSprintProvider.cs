using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputActionSprintProvider : SprintInputProvider
{
    private SprintButton SprintButton => SprintButton.Instance;

    [Header("Input")]
    [SerializeField] private PlayerInputController playerInputController;

    private void OnEnable()
    {
        playerInputController.SprintIA.started += HandleSprintPressed;
        playerInputController.SprintIA.canceled += HandleSprintCanceled;
    }

    private void OnDisable()
    {
        playerInputController.SprintIA.started -= HandleSprintPressed;
        playerInputController.SprintIA.canceled -= HandleSprintCanceled;
    }

    private void HandleSprintPressed(InputAction.CallbackContext ctx)
    {
        if (SprintButton == null || SprintButton.Button == null || !SprintButton.gameObject.activeInHierarchy)
            return;

        var pointerData = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(SprintButton.Button.gameObject, pointerData, ExecuteEvents.pointerDownHandler);
    }

    private void HandleSprintCanceled(InputAction.CallbackContext ctx)
    {
        if (SprintButton == null || SprintButton.Button == null || !SprintButton.gameObject.activeInHierarchy)
            return;

        var pointerData = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(SprintButton.Button.gameObject, pointerData, ExecuteEvents.pointerUpHandler);
        ExecuteEvents.Execute(SprintButton.Button.gameObject, pointerData, ExecuteEvents.pointerClickHandler);
    }
}