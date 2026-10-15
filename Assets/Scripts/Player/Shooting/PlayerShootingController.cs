using UnityEngine;

public class PlayerShootingController : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private PlayerWeaponController weaponController;
    [SerializeField] private PlayerShootTargetController shootingTargetController;
    [SerializeField] private CharacterRotationController rotationController;

    [Header("Params")]
    [SerializeField] private float shootRotationThreshold = 0.9f;

    private Weapon Weapon => weaponController.SpawnedWeapon;

    private void Update()
    {
        if (Weapon == null) return;

        if (rotationController.RotationProgress >= shootRotationThreshold) {
            Weapon.TryShootTarget(shootingTargetController.CurrentTarget);
        }
    }
}