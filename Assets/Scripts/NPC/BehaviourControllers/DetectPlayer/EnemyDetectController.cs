using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyDetectController : MonoBehaviour
{
    private EnemyDetectManager DetectManager => EnemyDetectManager.Instance;

    [Header("Main")]
    [SerializeField] private SphereCollider detectCollider;
    [SerializeField] private Transform detectionStartPoint;

    [Header("Params")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float detectionAngle = 90f;

    [SerializeField] private float findFrequency = 0.2f;
    private float currentFindTime = 0f;

    private List<IEnemyTarget> triggeredTargets = new();

    public event Action<IEnemyTarget> OnDetected;

    private void OnEnable()
    {
        DetectManager.RegisterController(this);
    }

    private void OnDisable()
    {
        DetectManager.UnregisterController(this);
    }

    private void Start()
    {
        detectCollider.radius = detectionRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        var target = other.GetComponent<IEnemyTarget>();
        if (target == null) return;

        triggeredTargets.Add(target);
    }

    private void OnTriggerExit(Collider other)
    {
        var target = other.GetComponent<IEnemyTarget>();
        if (target == null) return;

        triggeredTargets.Remove(target);
    }

    public void Tick()
    {
        currentFindTime += Time.deltaTime;

        if (currentFindTime >= findFrequency) {
            TryDetect();
            currentFindTime = 0f;
        }
    }

    private bool TryDetect()
    {
        var target = GetNearestTarget();
        if (target == null) return false;
        if (target is MonoBehaviour targetMono && targetMono == null) return false;

        OnDetected?.Invoke(target);

        return true;

    }

    private List<IEnemyTarget> GetAllVisibleTargets()
    {
        var targets = new List<IEnemyTarget>();

        for (int i = 0; i < triggeredTargets.Count; i++) {
            var target = triggeredTargets[i];
            if (target == null) continue;
            if (target is MonoBehaviour targetMono && targetMono == null) continue;

            var targetPosition = target.transform.position;
            var directionToTarget = targetPosition - detectionStartPoint.position;
            var distance = directionToTarget.magnitude;

            var angle = Vector3.Angle(transform.forward, directionToTarget.normalized);
            if (angle > detectionAngle * 0.5f)
                continue;

            if (Physics.Raycast(detectionStartPoint.position, directionToTarget.normalized, out var hit, distance)) {
                if (hit.collider.transform.root == target.transform.root) {
                    targets.Add(target);
                }
            }
        }

        return targets;
    }

    private IEnemyTarget GetNearestTarget()
    {
        var visibleTargets = GetAllVisibleTargets();
        IEnemyTarget nearestTarget = null;
        var minDistance = float.MaxValue;

        for (int i = 0; i < visibleTargets.Count; i++) {
            var target = visibleTargets[i];
            if (target == null) continue;
            if (target is MonoBehaviour targetMono && targetMono == null) continue;

            var distance = Vector3.Distance(target.transform.position, transform.position);

            if (distance >= minDistance)
                continue;

            nearestTarget = target;
            minDistance = distance;
        }

        return nearestTarget;
    }
}