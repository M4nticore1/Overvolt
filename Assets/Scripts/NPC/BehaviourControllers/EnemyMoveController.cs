using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private EnemyDetectController detectTargetController;

    private void OnEnable()
    {
        detectTargetController.OnDetected += HandleTargetDetected;
    }

    private void OnDisable()
    {
        detectTargetController.OnDetected -= HandleTargetDetected;
    }

    private void HandleTargetDetected(IEnemyTarget target)
    {
        Debug.Log("Detected");
        navAgent.SetDestination(target.transform.position);
    }
}