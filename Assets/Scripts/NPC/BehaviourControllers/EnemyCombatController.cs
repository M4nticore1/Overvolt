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

    [SerializeField] private bool isAttacking = false;
    [SerializeField] private float currentAttackTime = 0f;

    [SerializeField] private bool blockStopAttacking = false;

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
            TryStopAttakingCurrentTarget();
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

    // ----- Attacking -----
    public void TryStartAttackingTarget(IEnemyTarget target, bool blockStopAttacking = false)
    {
        if (target == null) return;
        if (target is MonoBehaviour targetMono && targetMono == null) return;

        Debug.Log("TryAttack");
        if (Enemy.CurrentTarget == null || Enemy.CurrentTarget == target) {
            Debug.Log("TryAttack1");
            Enemy.SetTarget(target);
            StartAttacking(blockStopAttacking);
        }
    }

    private void AttackCurrentTarget()
    {
        if (Enemy.CurrentTarget == null) return;

        Enemy.CurrentTarget.HealthComponent.TakeDamage(damage);

        OnAttacked?.Invoke();
    }

    private bool TryStartAttakingCurrentTarget(bool blockStopAttacking = false)
    {
        if (ShouldStartAttacking()) {
            StartAttacking(blockStopAttacking);
            return true;
        }

        return false;
    }

    private void StartAttacking(bool blockStopAttacking = false)
    {
        this.blockStopAttacking = blockStopAttacking;
        isAttacking = true;
    }

    private bool ShouldStartAttacking()
    {
        if (Enemy.CurrentTarget == null) return false;
        if (movement.IsMoving) return false;

        if (movement.IsReachedPosition(Enemy.CurrentTarget.transform.position)) return true;

        return false;
    }

    // ----- Stop Attaking -----
    private void TryStopAttakingCurrentTarget()
    {
        if (blockStopAttacking) return;

        isAttacking = false;
    }

    // ----- Events -----
    private void HandleTargetRemoved(IEnemyTarget target)
    {
        //TryStopAttakingCurrentTarget();
    }

    private void HandleMovementStarted()
    {
        //TryStopAttakingCurrentTarget();
    }

    private void HandleDestinationReached()
    {
        //TryStartAttakingCurrentTarget();
    }

    private void HandleDied()
    {
        //TryStopAttakingCurrentTarget();
    }
}