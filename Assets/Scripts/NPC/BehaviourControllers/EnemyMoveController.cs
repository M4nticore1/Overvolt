using UnityEngine;

public class EnemyMoveController : EnemyBehaviourController
{
    [Header("Movement")]
    [SerializeField] private NPCMovement movement;
    [SerializeField] private EnemyDetectController detectTargetController;

    [Header("Params")]
    [SerializeField] private float updateDestinationFrequency = 0.5f;
    private float currentUpdateDestinationTime = 0f;

    protected override void Subscribe()
    {
        base.Subscribe();

        Enemy.HealthComponent.OnDied += HandleDied;

        Enemy.OnTargetSet += HandleTargetSet;
        Enemy.OnTargetRemoved += HandleTargetRemoved;
    }

    protected override void Unsubscribe()
    {
        base.Unsubscribe();

        Enemy.HealthComponent.OnDied -= HandleDied;

        Enemy.OnTargetSet -= HandleTargetSet;
        Enemy.OnTargetRemoved -= HandleTargetRemoved;
    }

    public override void Tick()
    {
        if (Enemy.CurrentTarget != null) {
            currentUpdateDestinationTime += Time.deltaTime;

            if (currentUpdateDestinationTime >= updateDestinationFrequency) {
                MoveToCurrentTarget();
                currentUpdateDestinationTime = 0;
            }
        }
        else {
            currentUpdateDestinationTime = 0f;
        }
    }

    private void HandleDied()
    {
        StopMoving();
    }

    private void HandleTargetSet(IEnemyCombatTarget target)
    {
        MoveToCurrentTarget();
    }

    private void HandleTargetRemoved(IEnemyCombatTarget target)
    {
        MoveToCurrentTarget();
    }

    private void MoveToCurrentTarget()
    {
        if (!Enemy.HealthComponent.IsAlive) return;

        if (Enemy.CurrentTarget is MonoBehaviour targetMono && targetMono != null) {
            movement.TryMoveTo(Enemy.CurrentTarget.transform.position);
        }
    }

    private void StopMoving()
    {
        movement.TryStopMoving();
    }
}