using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTrigger : MonoBehaviour
{
    [SerializeField] private GameObject enemyTargetGameObject;
    [SerializeField] private Collider triggerCollider;

    private IEnemyCombatTarget enemyTarget;
    private readonly Dictionary<Collider, EnemyCombatController> activeControllers = new Dictionary<Collider, EnemyCombatController>();

    private void Awake()
    {
        if (enemyTargetGameObject != null) {
            enemyTarget = enemyTargetGameObject.GetComponent<IEnemyCombatTarget>();
        }

        if (enemyTarget == null) {
            Debug.LogError($"EnemyTarget is not valid or missing IEnemyTarget on {name}!", this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TryRegisterAndAttack(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TryRegisterAndAttack(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == null) return;

        activeControllers.Remove(other);
    }

    private void TryRegisterAndAttack(Collider other)
    {
        if (other == null) return;

        if (!activeControllers.TryGetValue(other, out var combatController)) {
            if (other.TryGetComponent(out combatController)) {
                activeControllers.Add(other, combatController);
            }
        }

        if (combatController != null) {
            combatController.TryStartAttackingTarget(enemyTarget, true);
        }
    }
}