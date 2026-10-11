using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionMoveInputProvider : MoveInputProvider
{
    [Header("Input Action")]
    [SerializeField] private PlayerInputController playerInputController;

    private void OnEnable()
    {
        playerInputController.MoveIA.performed += HandleMoveChanged;
        playerInputController.MoveIA.canceled += HandleMoveChanged;
    }

    private void OnDisable()
    {
        playerInputController.MoveIA.performed -= HandleMoveChanged;
        playerInputController.MoveIA.canceled -= HandleMoveChanged;
    }

    private void HandleMoveChanged(InputAction.CallbackContext ctx)
    {
        SetMoveInput(ctx.ReadValue<Vector2>());
    }
}