using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IEnemyTarget
{
    private static PlayerCharacter instance;
    public static PlayerCharacter Instance => instance ??= FindFirstObjectByType<PlayerCharacter>();

    [SerializeField] private HealthComponent healthComponent;
    public HealthComponent HealthComponent => healthComponent;

    [Header("IEnemyTarget")]
    [SerializeField] private Transform enemyTargetDetectPoint;
    public Transform EnemyTargetDetectPoint => enemyTargetDetectPoint;

    private void Awake()
    {
        if (instance != null && instance != this) {
            Debug.LogError("Instance is not null");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
}