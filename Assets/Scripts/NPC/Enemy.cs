using UnityEngine;

public class Enemy : MonoBehaviour, IPlayerTarget
{
    [SerializeField] private Collider hitCollider;
    public Collider HitCollider => hitCollider;

    [SerializeField] private HealthComponent healthComponent;
    public HealthComponent HealthComponent => healthComponent;

    [SerializeField] private float destroyTime = 1f;
    private float currentDestroyTime = 0;

    [Header("IPlayerTarget")]
    [SerializeField] private Transform playerTargetMarkerTransform;
    public Transform PlayerTargetMarkerTransform => playerTargetMarkerTransform;

    private void OnEnable()
    {
        healthComponent.OnDied += HandleDied;
    }

    private void OnDisable()
    {
        healthComponent.OnDied -= HandleDied;
    }

    private void Update()
    {
        TryDestroy();
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
        hitCollider.enabled = false;
    }
}