using System;
using UnityEngine;

[Serializable]
public class WeaponControllerData
{
    public WeaponId WeaponId { get; private set; }

    public WeaponControllerData Create(PlayerWeaponController controller)
    {
        if (controller == null) {
            Debug.LogError("WeaponController is not valid!");
            return null;
        }

        var weapon = controller.SpawnedWeapon;

        return new WeaponControllerData {
            WeaponId = weapon != null ? weapon.WeaponId : (WeaponId)Enum.GetValues(typeof(WeaponId)).GetValue(0),
        };
    }
}