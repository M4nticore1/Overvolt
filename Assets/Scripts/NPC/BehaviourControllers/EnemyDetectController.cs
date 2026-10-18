using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(SphereCollider))]
public class EnemyDetectController : EnemyBehaviourController
{
    private Headquater Headquater => Headquater.Instance;

    [Header("Detect")]
    [SerializeField] private SphereCollider detectCollider;
    [SerializeField] private Transform detectionStartPoint;

    [Header("Params")]
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float detectionAngle = 90f;
    [SerializeField] private float findFrequency = 0.2f;
    [SerializeField] private float undetectingTime = 1f;

    private float currentDetectTime = 0f;
    private float currentUndetectingTime = 0f;
    private bool isTargetOnView = false;

    private readonly List<IEnemyTarget> triggeredTargets = new();
    private readonly Dictionary<Collider, IEnemyTarget> cachedTargets = new();

    public event Action<IEnemyTarget> OnDetected;
    public event Action<IEnemyTarget> OnUndetected;

    private void Start()
    {
        detectCollider.radius = detectionRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        if (!cachedTargets.TryGetValue(other, out var target)) {
            target = other.GetComponent<IEnemyTarget>();
            if (target == null) return;
            cachedTargets[other] = target;
        }

        if (ShouldAddTarget(target)) {
            AddTriggeredTarget(target);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (cachedTargets.TryGetValue(other, out var target)) {
            if (ShouldAddTarget(target)) {
                AddTriggeredTarget(target);
            }
            else {
                RemoveTriggeredTarget(target);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (cachedTargets.TryGetValue(other, out var target)) {
            RemoveTriggeredTarget(target);
            cachedTargets.Remove(other);
        }
    }

    protected override void Subscribe()
    {
        base.Subscribe();

        Enemy.HealthComponent.OnDied += HandleDied;
    }

    protected override void Unsubscribe()
    {
        base.Unsubscribe();

        Enemy.HealthComponent.OnDied -= HandleDied;
    }

    public override void Tick()
    {
        currentDetectTime += Time.deltaTime;

        if (currentDetectTime >= findFrequency) {
            if (!TryDetect()) {
                isTargetOnView = false;
            }

            currentDetectTime = 0f;
        }

        if (!isTargetOnView) {
            currentUndetectingTime += Time.deltaTime;

            if (currentUndetectingTime >= undetectingTime) {
                Undetect();
                currentUndetectingTime = 0f;
            }
        }
        else {
            currentUndetectingTime = 0f;
        }
    }

    private void AddTriggeredTarget(IEnemyTarget target)
    {
        if (target is MonoBehaviour targetMono && targetMono == null) return;

        if (!triggeredTargets.Contains(target)) {
            triggeredTargets.Add(target);
        }
    }

    private void RemoveTriggeredTarget(IEnemyTarget target)
    {
        triggeredTargets.Remove(target);
    }

    private void ClearTriggeredTargets()
    {
        triggeredTargets.Clear();
        cachedTargets.Clear();
    }

    private bool TryDetect()
    {
        var target = GetNearestTarget();

        if (target == null) return false;
        if (target is MonoBehaviour targetMono && targetMono == null) return false;
        if (target == Enemy.CurrentTarget) return true;

        isTargetOnView = true;
        Enemy.SetTarget(target);
        OnDetected?.Invoke(target);

        return true;
    }

    private void Undetect()
    {
        var lastTarget = Enemy.CurrentTarget;
        Enemy.SetTarget(Headquater);

        OnUndetected?.Invoke(lastTarget);
    }

    private void HandleDied()
    {
        ClearTriggeredTargets();
    }

    private List<IEnemyTarget> GetAllVisibleTargets()
    {
        var targets = new List<IEnemyTarget>();

        for (int i = 0; i < triggeredTargets.Count; i++) {
            var target = triggeredTargets[i];
            if (target == null) continue;
            if (target is MonoBehaviour targetMono && targetMono == null) continue;

            var targetPosition = target.EnemyTargetDetectPoint.position;
            var directionToTarget = targetPosition - detectionStartPoint.position;
            var distance = directionToTarget.magnitude;

            var angle = Vector3.Angle(transform.forward, directionToTarget.normalized);
            if (angle > detectionAngle * 0.5f)
                continue;

            if (Physics.Raycast(detectionStartPoint.position, directionToTarget.normalized, out var hit, distance)) {
                if (hit.collider.transform == target.transform) {
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

    private bool ShouldAddTarget(IEnemyTarget target)
    {
        if (target == null) return false;
        if (target is MonoBehaviour targetMono && targetMono == null) return false;
        if (target.HealthComponent && !target.HealthComponent.IsAlive) return false;

        if (!Enemy.HealthComponent.IsAlive) return false;

        return true;
    }
}