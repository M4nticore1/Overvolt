using UnityEngine;

public interface IEnemyCombatTarget
{
    public Transform transform { get; }
    public Transform EnemyTargetDetectPoint { get; }

    public HealthComponent HealthComponent { get; }
}
