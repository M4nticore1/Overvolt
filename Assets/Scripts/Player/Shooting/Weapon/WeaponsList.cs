using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponsList", menuName = "Scriptable Objects/WeaponsList")]
public class WeaponsList : ScriptableObject
{
    private static WeaponsList instance;
    public static WeaponsList Instance {
        get {
            if (instance == null) {
                instance = Resources.Load<WeaponsList>("Lists/WeaponsList");
            }

            return instance;
        }
    }

    [SerializeField] private List<Weapon> weaponPrefabs;

    private Dictionary<WeaponId, Weapon> weaponPrefabsDict;
    private IReadOnlyDictionary<WeaponId, Weapon> WeaponPrefabsDict {
        get {
            if (weaponPrefabsDict == null) {
                weaponPrefabsDict = new();

                for (int i = 0; i < weaponPrefabs.Count; i++) {
                    var prefab = weaponPrefabs[i];
                    if (prefab == null) continue;

                    weaponPrefabsDict.Add(prefab.WeaponId, prefab);
                }
            }

            return weaponPrefabsDict;
        }
    }

    public Weapon GetWeapon(WeaponId id)
    {
        WeaponPrefabsDict.TryGetValue(id, out var weapon);

        return weapon;
    }
}