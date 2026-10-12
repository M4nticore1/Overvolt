using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ShootingTargetFinder : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private PlayerShootingController shootingController;
    [SerializeField] private Collider findCollider;

    [Header("Params")]
    [SerializeField] private float findTargetFrequency = 1f;

    private List<IPlayerTarget> targetsList = new();
    private IPlayerTarget lastPlayerTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        var playerTarget = other.GetComponent<IPlayerTarget>();
        if (playerTarget == null) return;

        AddTarget(playerTarget);
    }

    private void OnTriggerExit(Collider other)
    {
        var playerTarget = other.GetComponent<IPlayerTarget>();
        if (playerTarget == null) return;

        RemoveTarget(playerTarget);
    }

    private void Update()
    {
        float minDistance = Mathf.Infinity;
        IPlayerTarget nearestTarget = null;

        for (int i = targetsList.Count - 1; i >= 0; i--) {
            var target = targetsList[i];

            if (target == null) {
                targetsList.RemoveAt(i);
                continue;
            }

            var distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance > minDistance) {
                continue;
            }

            minDistance = distance;
            nearestTarget = target;
        }

        if (nearestTarget != null) {
            lastPlayerTarget = nearestTarget;
            shootingController.SetTarget(nearestTarget);
        }
        else if (lastPlayerTarget != null) {
            shootingController.RemoveTarget(lastPlayerTarget);
        }
    }

    private void AddTarget(IPlayerTarget target)
    {
        if (target == null) return;

        if (!targetsList.Contains(target)) {
            targetsList.Add(target);
        }
    }

    private void RemoveTarget(IPlayerTarget target)
    {
        targetsList.Remove(target);
    }
}