using System.Collections.Generic;
using UnityEngine;

public class ButtonScaleManager : MonoBehaviour
{
    private static ButtonScaleManager instance;
    public static ButtonScaleManager Instance => instance ??= FindFirstObjectByType<ButtonScaleManager>();

    private List<ButtonScaleController> controllers = new();

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        for (int i = controllers.Count - 1; i >= 0; --i) {
            var controller = controllers[i];
            if (controller == null) {
                controllers.RemoveAt(i);
                continue;
            }

            controller.Tick();
        }
    }

    public void RegisterController(ButtonScaleController controller)
    {
        if (controller == null) {
            Debug.LogError("Controller is not valid!");
        }

        controllers.Add(controller);
    }

    public void UnregisterController(ButtonScaleController controller)
    {
        if (controller == null) {
            Debug.LogError("Controller is not valid!");
        }

        controllers.Remove(controller);
    }
}