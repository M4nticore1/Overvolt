using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IPlayerTarget
{
    private EnemiesManager EnemiesManager => EnemiesManager.Instance;

    [SerializeField] private EnemyBehaviourController[] behaviourControllers;

    [SerializeField] private Collider hitCollider;
    public Collider HitCollider => hitCollider;

    [SerializeField] private HealthComponent healthComponent;
    public HealthComponent HealthComponent => healthComponent;

    [SerializeField] private float destroyTime = 1f;
    private float currentDestroyTime = 0;

    [Header("IPlayerTarget")]
    [SerializeField] private Transform playerTargetShootPoint;
    public Transform PlayerTargetShootPoint => playerTargetShootPoint;

    [SerializeField] private Transform playerTargetMarkerPoint;
    public Transform PlayerTargetMarkerPoint => playerTargetMarkerPoint;

    public IEnemyTarget CurrentTarget { get; private set; }

    public event Action<IEnemyTarget> OnTargetSet;
    public event Action<IEnemyTarget> OnTargetRemoved;

    private void OnEnable()
    {
        if (EnemiesManager != null) {
            EnemiesManager.RegisterEnemy(this);
        }

        if (healthComponent != null) {
            healthComponent.OnDied += HandleDied;
        }
    }

    private void OnDisable()
    {
        if (EnemiesManager != null) {
            EnemiesManager.UnregisterEnemy(this);
        }

        if (healthComponent != null) {
            healthComponent.OnDied -= HandleDied;
        }
    }

    public void Tick()
    {
        foreach (var controller in behaviourControllers) {
            if (controller == null) continue;

            controller.Tick();
        }

        TryDestroy();
    }

    public void SetTarget(IEnemyTarget target)
    {
        if (CurrentTarget == target) return;

        var lastTarget = CurrentTarget;

        if (target is MonoBehaviour targetMono && targetMono != null) {
            CurrentTarget = target;
            OnTargetSet?.Invoke(target);
        }
        else {
            CurrentTarget = null;
            OnTargetRemoved?.Invoke(lastTarget);
        }
    }

    private void TryDestroy()
    {
        if (healthComponent == null) return;
        if (healthComponent.IsAlive) return;

        currentDestroyTime += Time.deltaTime;

        if (currentDestroyTime >= destroyTime) {
            Destroy(gameObject);
        }
    }

    private void HandleDied()
    {
        SetTarget(null);

        if (hitCollider != null) {
            hitCollider.enabled = false;
        }
    }
}