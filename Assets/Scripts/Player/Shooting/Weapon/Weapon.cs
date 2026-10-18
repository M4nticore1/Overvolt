using System;
using UnityEngine;

public enum WeaponId
{
    Ratchet
}

public struct BulletShotData
{
    public Vector3 StartPosition;
    public Vector3 Direction;
    public float Distance;
}

public class Weapon : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private WeaponId weaponId;
    public WeaponId WeaponId => weaponId;

    [SerializeField] private Transform muzzlePoint;
    public Transform MuzzlePoint => muzzlePoint;

    [Header("Damage")]
    [SerializeField] private float damage = 10;
    public float Damage => damage;

    [SerializeField] private float minDamage = 5;
    public float MinDamage => minDamage;

    [Header("Sperad")]
    [SerializeField] private float maxSpreadAngle = 5;
    public float MaxSpreadAngle => maxSpreadAngle;

    [Header("Distance")]
    [SerializeField] private float shootDistance = 10;
    public float ShootDistance => shootDistance;

    [Tooltip("Curvature coefficient. Greater than 1 — damage drops slowly at first and sharply at the end.")]
    [SerializeField] private float damageFalloffExponent = 1f;
    public float DamageFalloffExponent => damageFalloffExponent;

    [Header("Fire")]
    [SerializeField] private float fireRate = 600;
    public float FireRate => fireRate;
    private float nextFireTime = 0f;

    [SerializeField] private int bulletsCount = 1;
    public int BulletsCount => bulletsCount;

    [Header("VFX")]
    [SerializeField] private WeaponTracer weaponTracer;
    public WeaponTracer WeaponTracer => weaponTracer;

    public event Action<BulletShotData> OnBulletShot;
    public event Action OnShot;

    public bool TryShootTarget(IPlayerTarget target)
    {
        if (Time.time < nextFireTime) return false;

        ShootTarget(target);

        return true;
    }

    private void ShootTarget(IPlayerTarget target)
    {
        var targetPoint = Vector3.zero;

        if (target is MonoBehaviour targetMono && targetMono != null) {
            targetPoint = target.PlayerTargetShootPoint.position;
        }
        else {
            targetPoint = muzzlePoint.position + muzzlePoint.forward * shootDistance;
        }

        for (int i = 0; i < bulletsCount; i++) {
            var shootDirection = (targetPoint - muzzlePoint.position).normalized;

            var spreadRotation = Quaternion.Euler(GetRandomSpread(), GetRandomSpread(), 0f);
            shootDirection = spreadRotation * shootDirection;

            if (Physics.Raycast(muzzlePoint.position, shootDirection, out var hit, ShootDistance)) {
                var damageables = hit.collider.GetComponents<IDamageable>();

                for (int j = 0; j < damageables.Length; j++) {
                    var damageable = damageables[j];
                    if (damageable is MonoBehaviour damageableMono && damageableMono == null) continue;
                    
                    damageable.TakeDamage(GetDamageAtDistance(hit.distance));
                }
            }

            var shotData = new BulletShotData {
                StartPosition = muzzlePoint.position,
                Direction = shootDirection,
                Distance = hit.distance
            };

            OnBulletShot?.Invoke(shotData);
        }

        nextFireTime = Time.time + (1 / (fireRate / 60));

        OnShot?.Invoke();
    }

    private float GetDamageAtDistance(float distance)
    {
        var t = Mathf.Clamp01(distance / shootDistance);   
        var multiplier = 1f - Mathf.Pow(t, damageFalloffExponent);

        return Mathf.Max(damage * Mathf.Max(0f, multiplier), minDamage);
    }

    private float GetRandomSpread()
    {
        return UnityEngine.Random.Range(-maxSpreadAngle, MaxSpreadAngle);
    }
}