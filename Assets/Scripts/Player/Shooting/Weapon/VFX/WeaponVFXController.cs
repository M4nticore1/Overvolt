using UnityEngine;

public class WeaponVFXController : MonoBehaviour
{
    [SerializeField] private Weapon weapon;

    private void OnEnable()
    {
        weapon.OnBulletShot += HandleBulletShot;
    }

    private void OnDisable()
    {
        weapon.OnBulletShot -= HandleBulletShot;
    }

    private void HandleBulletShot(BulletShotData data)
    {
        var rotation = Quaternion.LookRotation(data.Direction);
        Instantiate(weapon.WeaponTracer, data.StartPosition, rotation);
    }
}