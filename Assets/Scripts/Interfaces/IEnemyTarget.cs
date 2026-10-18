using UnityEngine;

public interface IEnemyTarget
{
    public Transform transform { get; }
    public Transform EnemyTargetDetectPoint { get; }

    public HealthComponent HealthComponent { get; }
}
