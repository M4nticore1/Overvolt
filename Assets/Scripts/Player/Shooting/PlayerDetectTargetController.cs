using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PlayerDetectTargetController : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private PlayerWeaponController weaponController;
    [SerializeField] private PlayerShootTargetController shootingTargetController;
    [SerializeField] private SphereCollider findingTrigger;
    [SerializeField] private Transform detectTargetStartPoint;

    [Header("Params")]
    [SerializeField] private float findTargetFrequency = 1f;
    private float findTimeTime = 0f;

    private List<IPlayerCombatTarget> targetsList = new();
    private IPlayerCombatTarget lastTarget;

    private void OnEnable()
    {
        weaponController.OnWeaponChanged += HandleWeaponChanged;
    }

    private void OnDisable()
    {
        weaponController.OnWeaponChanged -= HandleWeaponChanged;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        var playerTarget = other.GetComponent<IPlayerCombatTarget>();
        if (playerTarget == null) return;

        AddTarget(playerTarget);
    }

    private void OnTriggerExit(Collider other)
    {
        var playerTarget = other.GetComponent<IPlayerCombatTarget>();
        if (playerTarget == null) return;

        RemoveTarget(playerTarget);
    }

    private void Update()
    {
        findTimeTime += Time.deltaTime;

        if (findTimeTime >= findTargetFrequency) {
            float minDistance = Mathf.Infinity;
            IPlayerCombatTarget nearestTarget = null;

            for (int i = targetsList.Count - 1; i >= 0; i--) {
                var target = targetsList[i];
                if (target is MonoBehaviour targetMono && targetMono == null) {
                    targetsList.RemoveAt(i);
                    continue;
                }

                var distance = Vector3.Distance(detectTargetStartPoint.position, target.PlayerTargetShootPoint.position);
                var direction = (target.PlayerTargetShootPoint.position - detectTargetStartPoint.position).normalized;

                if (Physics.Raycast(detectTargetStartPoint.position, direction, out var hit, distance)) {
                    var hittedTarget = hit.collider.GetComponent<IPlayerCombatTarget>();

                    if (hittedTarget != target) continue;
                }

                if (target.HealthComponent != null && !target.HealthComponent.IsAlive) continue;       
                if (distance > minDistance) continue;

                minDistance = distance;
                nearestTarget = target;
            }

            if (nearestTarget != null) {
                lastTarget = nearestTarget;
                shootingTargetController.SetTarget(nearestTarget);
            }
            else if (lastTarget != null) {
                shootingTargetController.RemoveTarget(lastTarget);
            }

            findTimeTime = 0f;
        }
    }

    private void SetFindingRadius(float radius)
    {
        findingTrigger.radius = radius;
    }

    private void AddTarget(IPlayerCombatTarget target)
    {
        if (target == null) return;

        if (!targetsList.Contains(target)) {
            targetsList.Add(target);
        }
    }

    private void RemoveTarget(IPlayerCombatTarget target)
    {
        targetsList.Remove(target);
    }

    private void HandleWeaponChanged(Weapon weapon)
    {
        if (weapon == null) return;

        SetFindingRadius(weapon.ShootDistance);
    }
}