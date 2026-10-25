using UnityEngine;

public abstract class MoveInputProvider : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private PlayerMoveController playerMoveController;

    protected void SetMoveInput(Vector2 moveInput)
    {
        playerMoveController.SetMoveInput(moveInput);
    }
}