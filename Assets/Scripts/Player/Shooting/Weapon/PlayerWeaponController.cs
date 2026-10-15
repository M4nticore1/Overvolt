using System;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private static PlayerWeaponController instance;
    public static PlayerWeaponController Instance => instance ??= FindFirstObjectByType<PlayerWeaponController>();

    [Header("Main")]
    [SerializeField] private WeaponsList weaponsList;
    private PlayerDataManager PlayerDataManager => PlayerDataManager.Instance;

    [Header("Rapams")]
    [SerializeField] private WeaponId defaultWeaponId;
    [SerializeField] private Transform weaponHolder;

    public Weapon SpawnedWeapon {  get; private set; }

    public event Action<Weapon> OnWeaponChanged;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        var playerData = PlayerDataManager.PlayerData;
        var weaponData = playerData?.Weapon;

        if (weaponData != null) {
            SetWeaponAndSpawn(weaponData.WeaponId);
        }
        else {
            SetWeaponAndSpawn(defaultWeaponId);
        }
    }

    public void SetWeaponAndSpawn(WeaponId weaponId)
    {
        var weaponPrefab = weaponsList.GetWeapon(weaponId);

        DestroyWeapon();
        SpawnWeapon(weaponPrefab);

        OnWeaponChanged?.Invoke(SpawnedWeapon);
    }

    public void SetWeaponAndSpawn(Weapon weaponPrefab)
    {
        DestroyWeapon();
        SpawnWeapon(weaponPrefab);
    }

    private void SpawnWeapon(Weapon weaponPrefab)
    {
        if (weaponPrefab == null) {
            Debug.LogError("WeaponPrefab is not valid!");
            return;
        }

        SpawnedWeapon = Instantiate(weaponPrefab, weaponHolder);
    }

    private void DestroyWeapon()
    {
        if (SpawnedWeapon == null) return;

        Destroy(SpawnedWeapon.gameObject);
    }
}