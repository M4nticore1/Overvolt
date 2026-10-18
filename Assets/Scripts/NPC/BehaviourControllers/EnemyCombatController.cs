using System;
using UnityEngine;

public class EnemyCombatController : EnemyBehaviourController
{
    [Header("Combaе")]
    [SerializeField] private NPCMovement movement;
    [SerializeField] private HealthComponent healthComponent;

    [Header("Params")]
    [SerializeField] float damage = 5f;
    [SerializeField] float attackFrequency = 1f;

    private float currentAttackTime = 0f;
    private bool isAttacking = false;

    public event Action OnAttacked;

    protected override void Subscribe()
    {
        base.Subscribe();

        Enemy.OnTargetRemoved += HandleTargetRemoved;
        movement.OnMovementStarted += HandleMovementStarted;
        movement.OnDestinationReached += HandleDestinationReached;
        healthComponent.OnDied += HandleDied;
    }

    protected override void Unsubscribe()
    {
        base.Unsubscribe();

        Enemy.OnTargetRemoved -= HandleTargetRemoved;
        movement.OnMovementStarted -= HandleMovementStarted;
        movement.OnDestinationReached -= HandleDestinationReached;
        healthComponent.OnDied -= HandleDied;
    }

    public override void Tick()
    {
        if (!TryStartAttakingCurrentTarget()) {
            StopAttakingCurrentTarget();
        }

        if (isAttacking) {
            currentAttackTime += Time.deltaTime;

            if (currentAttackTime >= attackFrequency) {
                AttackCurrentTarget();
                currentAttackTime = 0f;
            }
        }
        else {
            currentAttackTime = 0f;
        }
    }

    private void AttackCurrentTarget()
    {
        if (Enemy.CurrentTarget == null) return;

        Enemy.CurrentTarget.HealthComponent.TakeDamage(damage);

        OnAttacked?.Invoke();
    }

    private bool TryStartAttakingCurrentTarget()
    {
        if (ShouldStartAttacking()) {
            isAttacking = true;
            return true;
        }

        return false;
    }

    private void StopAttakingCurrentTarget()
    {
        isAttacking = false;
    }

    private void HandleTargetRemoved(IEnemyTarget target)
    {
        StopAttakingCurrentTarget();
    }

    private void HandleMovementStarted()
    {
        StopAttakingCurrentTarget();
    }

    private void HandleDestinationReached()
    {
        TryStartAttakingCurrentTarget();
    }

    private void HandleDied()
    {
        StopAttakingCurrentTarget();
    }

    private bool ShouldStartAttacking()
    {
        if (Enemy.CurrentTarget == null) return false;

        if (movement.IsReachedPosition(Enemy.CurrentTarget.transform.position)) return true;

        return false;
    }
}