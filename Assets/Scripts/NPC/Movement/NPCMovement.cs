using System;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    private NPCMovementManager movementManager => NPCMovementManager.Instance;

    [Header("Main")]
    [SerializeField] private NavMeshAgent navAgent;
    public NavMeshAgent NavAgent => navAgent;

    [Header("Position")]
    [SerializeField] private float maxSampleDistance = 5f;

    [Header("Rotation")]
    [SerializeField] private bool useTargetRotation = true;
    [SerializeField] private float rotationSpeed = 1f;

    [Header("Debug")]
    [field: SerializeField] public bool IsMoving { get; private set; } = false;

    public Vector3? TargetPosition { get; private set; } = null;
    public Quaternion? TargetRotation { get; private set; } = null;

    public event Action OnMovementStarted;
    public event Action OnMovementStopped;
    public event Action OnDestinationReached;

    private void OnEnable()
    {
        if (movementManager != null) {
            movementManager.RegisterMovement(this);
        }
        else {
            Debug.LogError($"[{nameof(NPCMovement)}] Movement Manager is not valid!");
        }
    }

    private void OnDisable()
    {
        if (movementManager != null) {
            movementManager.UnregisterMovement(this);
        }
    }

    public void Tick()
    {
        if (IsMoving && IsDestinationReached()) {
            TryStopMoving();
        }

        ProcessRotation();
    }

    public void Move(Vector3 direction, float speed)
    {
        transform.position += direction * speed;
    }

    public void SetAgentEnabled(bool enabled)
    {
        if (navAgent != null) {
            navAgent.enabled = enabled;
        }
    }

    // Move To
    public bool TryMoveTo(Transform target)
    {
        if (target == null) {
            Debug.LogError($"[{nameof(NPCMovement)}] Target Transform is not valid!");
            return false;
        }

        if (!TryMoveTo(target.position)) return false;

        SetTargetRotation(target.rotation);

        return true;
    }

    public bool TryMoveTo(Vector3 position, bool useReachedPosition = true)
    {
        if (!CanStartMoving()) return false;

        if (NavMesh.SamplePosition(position, out NavMeshHit hit, maxSampleDistance, navAgent.areaMask)) {
            position = hit.position;
        }
        else {
            return false;
        }

        if (IsMoving && TargetPosition.HasValue && Vector3.SqrMagnitude(TargetPosition.Value - position) < 0.01f) {
            return true;
        }

        if (IsReachedPosition(position)) {
            IsMoving = false;
            TargetPosition = null;

            if (useReachedPosition) {
                OnDestinationReached?.Invoke();
                return true;
            }
            else {
                return false;
            }
        }

        IsMoving = true;
        navAgent.isStopped = false;
        TargetPosition = position;
        RemoveTargetRotation();

        if (!navAgent.SetDestination(position)) {
            TargetPosition = null;
            IsMoving = false;
            return false;
        }

        OnMovementStarted?.Invoke();

        return true;
    }

    // Stop
    public bool TryStopMoving()
    {
        if (!CanStopMoving()) return false;

        var destinationReached = IsDestinationReached();

        navAgent.isStopped = true;
        IsMoving = false;
        TargetPosition = null;

        if (destinationReached) {
            OnDestinationReached?.Invoke();
        }

        OnMovementStopped?.Invoke();

        return true;
    }

    // Check
    public bool CanStartMoving()
    {
        if (navAgent == null) return false;
        if (!navAgent.enabled) return false;
        if (!navAgent.isOnNavMesh) return false;

        return true;
    }

    public bool CanStopMoving()
    {
        if (navAgent == null) return false;
        if (!IsMoving) return false;
        if (!navAgent.enabled) return false;
        if (!navAgent.isOnNavMesh) return false;

        return true;
    }

    public bool IsDestinationReached()
    {
        if (TargetPosition == null) return false;

        return IsReachedPosition(TargetPosition.Value);
    }

    public bool IsReachedPosition(Vector3 targetPosition)
    {
        return GetDistanceToPosition(targetPosition) <= navAgent.stoppingDistance;
    }

    public bool CanReachPosition(Vector3 targetPosition)
    {
        if (navAgent == null) return false;
        if (!navAgent.enabled) return false;
        if (!navAgent.isOnNavMesh) return false;

        var path = new NavMeshPath();

        if (!navAgent.CalculatePath(targetPosition, path)) {
            return false;
        }

        return path.status == NavMeshPathStatus.PathComplete;
    }

    public float GetDistanceToPosition(Vector3 targetPosition)
    {
        var currentPosition = transform.position;

        return Vector3.Distance(new Vector3(currentPosition.x, currentPosition.y - navAgent.baseOffset, currentPosition.z), targetPosition);
    }

    public float? GetTargetPositionDistance()
    {
        if (TargetPosition == null) return null;

        return Vector3.Distance(
            navAgent.nextPosition,
            TargetPosition.Value
        );
    }

    // Rotation
    private void ProcessRotation()
    {
        if (!useTargetRotation) return;
        if (TargetRotation == null) return;

        transform.rotation = Quaternion.Lerp(transform.rotation, TargetRotation.Value, rotationSpeed * Time.deltaTime);
    }

    private void SetTargetRotation(Quaternion value)
    {
        TargetRotation = value;
    }

    private void RemoveTargetRotation()
    {
        TargetRotation = null;
    }

    public void SetUseRotation(bool value)
    {
        useTargetRotation = value;
    }
}