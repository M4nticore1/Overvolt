using UnityEngine;

public class Headquater : Building, IEnemyCombatTarget
{
    private static Headquater instance;
    public static Headquater Instance => instance ??= FindFirstObjectByType<Headquater>();

    [SerializeField] private HealthComponent healthComponent;
    public HealthComponent HealthComponent => healthComponent;

    [Header("IEnemyTarget")]
    [SerializeField] private Transform enemyTargetDetectPoint;
    public Transform EnemyTargetDetectPoint => enemyTargetDetectPoint;

    private void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
}