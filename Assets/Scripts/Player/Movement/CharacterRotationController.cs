using UnityEngine;

public class CharacterRotationController : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private Transform playerRoot;
    [SerializeField] private PlayerMoveController moveController;
    [SerializeField] private PlayerShootTargetController shootTargetController;

    [Header("Params")]
    [SerializeField] private float rotationSpeed = 10f;

    public float RotationProgress { get; private set; }

    private void Awake()
    {
        if (playerRoot == null)
            Debug.LogError("PlayerRoot is not valid!");
        if (moveController == null)
            Debug.LogError("MoveController is not valid!");
        if (shootTargetController == null)
            Debug.LogError("ShootTargetController is not valid!");
    }

    private void Update()
    {
        if (shootTargetController == null || moveController == null || playerRoot == null) return;

        var moveInput = moveController.MoveInput;
        var shootTarget = shootTargetController.CurrentTarget;

        Vector3 targetPosition;
        if (shootTarget is MonoBehaviour targetMono && targetMono != null) {
            targetPosition = targetMono.transform.position;
        }
        else if (moveInput.sqrMagnitude > 0.001f) {
            targetPosition = playerRoot.position + new Vector3(moveInput.x, 0, moveInput.y);
        }
        else {
            RotationProgress = Mathf.MoveTowards(RotationProgress, 0f, rotationSpeed * Time.deltaTime);
            return;
        }

        targetPosition.y = transform.position.y;
        var direction = targetPosition - transform.position;

        if (direction.sqrMagnitude > 0.001f) {
            var targetRotation = Quaternion.LookRotation(direction);
            var dot = Vector3.Dot(transform.forward, direction.normalized);

            RotationProgress = Mathf.Clamp01((dot + 1f) * 0.5f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}