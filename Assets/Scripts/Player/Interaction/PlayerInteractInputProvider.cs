using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInteractInputProvider : MonoBehaviour
{
    private InteractButton InteractButton => InteractButton.Instance;

    [SerializeField] private PlayerInputController inputController;

    private void OnEnable()
    {
        inputController.InteractIA.started += HandleInteractPressed;
        inputController.InteractIA.canceled += HandleInteractCanceled;
    }

    private void OnDisable()
    {
        inputController.InteractIA.started -= HandleInteractPressed;
        inputController.InteractIA.canceled -= HandleInteractCanceled;
    }

    private void HandleInteractPressed(InputAction.CallbackContext ctx)
    {
        if (InteractButton == null || InteractButton.Button == null || !InteractButton.gameObject.activeInHierarchy)
            return;

        var pointerData = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(InteractButton.Button.gameObject, pointerData, ExecuteEvents.pointerDownHandler);
    }

    private void HandleInteractCanceled(InputAction.CallbackContext ctx)
    {
        if (InteractButton == null || InteractButton.Button == null || !InteractButton.gameObject.activeInHierarchy)
            return;

        var pointerData = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(InteractButton.Button.gameObject, pointerData, ExecuteEvents.pointerUpHandler);
        ExecuteEvents.Execute(InteractButton.Button.gameObject, pointerData, ExecuteEvents.pointerClickHandler);
    }
}