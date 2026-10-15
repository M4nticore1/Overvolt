using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    private static PlayerDataManager instance;
    public static PlayerDataManager Instance => instance ??= FindFirstObjectByType<PlayerDataManager>();

    private PlayerData playerData;
    public PlayerData PlayerData => playerData != null ? playerData : null;

    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}