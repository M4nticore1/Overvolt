using System.Collections.Generic;
using UnityEngine;

public class NPCMovementManager : MonoBehaviour
{
    private static NPCMovementManager instance;
    public static NPCMovementManager Instance => instance ??= FindFirstObjectByType<NPCMovementManager>();

    private List<NPCMovement> movementComponents = new();
    public IReadOnlyList<NPCMovement> MovementComponents => movementComponents;

    private void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Update()
    {
        for (int i = movementComponents.Count - 1; i >= 0; i--) {
            var movement = movementComponents[i];
            if (movement == null) {
                movementComponents.RemoveAt(i);
                continue;
            }

            movement.Tick();
        }
    }

    public void RegisterMovement(NPCMovement movement)
    {
        if (movement == null) return;
        if (movementComponents.Contains(movement)) return;

        movementComponents.Add(movement);
    }

    public void UnregisterMovement(NPCMovement movement)
    {
        movementComponents.Remove(movement);
    }
}