using System;
using UnityEngine;

public class PlayerMoveController : MonoBehaviour
{
    private static PlayerMoveController instance;
    public static PlayerMoveController Instance => instance ??= FindFirstObjectByType<PlayerMoveController>();

    [Header("Main")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerInputController playerInputController;

    [Header("Speed")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float transitionSpeed = 10f;

    private float TargetSpeed => isSprinting ? sprintSpeed : walkSpeed;
    private float currentSpeed = 0f;

    public Vector2 MoveInput { get; private set; }
    public float MoveVelocity => MoveInput.magnitude;

    public bool IsMoving => MoveVelocity > 0;
    public bool isSprinting { get; private set; } = false;

    // Gravity
    private float gravity = -9.81f;
    private float gravityVelocity = 0f;

    public event Action<Vector2> OnMoveInputChanged;
    public event Action<bool> OnSprintActiveChanged;

    private void Awake()
    {
        instance = this;

        currentSpeed = walkSpeed;
    }

    private void Update()
    {
        UpdateMovementSpeed();
        UpdateGravity();
        PerformMove(MoveInput);
    }

    public void SetMoveInput(Vector2 moveInput)
    {
        MoveInput = moveInput;
        OnMoveInputChanged?.Invoke(moveInput);
    }

    public bool SetSprintActive(bool active)
    {
        if (active == isSprinting) {
            return false;
        }

        isSprinting = active;
        OnSprintActiveChanged?.Invoke(active);

        return true;
    }

    private void PerformMove(Vector2 direction)
    {
        var inputDir = new Vector3(MoveInput.x, 0, MoveInput.y);
        var cameraYaw = playerCamera.transform.eulerAngles.y;
        var horizontalMove = Quaternion.Euler(0, cameraYaw, 0) * inputDir * currentSpeed;

        var finalMove = horizontalMove;
        finalMove.y = gravityVelocity;

        characterController.Move(finalMove * Time.deltaTime);
    }

    private void UpdateGravity()
    {
        if (characterController.isGrounded) {
            gravityVelocity = 0f;
        }
        else {
            gravityVelocity += gravity * Time.deltaTime;
        }
    }

    private void UpdateMovementSpeed()
    {
        currentSpeed = Mathf.MoveTowards(currentSpeed, TargetSpeed, transitionSpeed * Time.deltaTime);
    }
}