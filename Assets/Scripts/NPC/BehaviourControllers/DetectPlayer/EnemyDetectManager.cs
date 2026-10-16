using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectManager : MonoBehaviour
{
    private static EnemyDetectManager instance;
    public static EnemyDetectManager Instance => instance ??= FindFirstObjectByType<EnemyDetectManager>();

    private List<EnemyDetectController> controllers = new();

    private void Awake()
    {
        if (instance != null && instance != this) {
            Debug.LogError("EnemyFindPlayerManager is not null!");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Update()
    {
        for (int i = controllers.Count - 1; i >= 0; i--) {
            var controller = controllers[i];

            if (controller == null) {
                controllers.RemoveAt(i);
                continue;
            }

            controller.Tick();
        }
    }

    public void RegisterController(EnemyDetectController controller)
    {
        if (controller == null) return;
        if (controllers.Contains(controller)) return;

        controllers.Add(controller);
    }

    public void UnregisterController(EnemyDetectController controller)
    {
        controllers.Remove(controller);
    }
}