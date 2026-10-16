using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IEnemyTarget
{
    private static PlayerCharacter instance;
    public static PlayerCharacter Instance => instance ??= FindFirstObjectByType<PlayerCharacter>();

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