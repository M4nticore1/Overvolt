using System.Collections.Generic;
using UnityEngine;

public class BillboardsManager : MonoBehaviour
{
    private static BillboardsManager instance;
    public static BillboardsManager Instance=> instance ??= FindFirstObjectByType<BillboardsManager>();

    private readonly List<Billboard> billboards = new();

    private void Awake()
    {
        if (instance) {
            Debug.LogError($"[{nameof(BillboardsManager)}] Another instance already exists in the scene! Destroying this.");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void Register(Billboard billboard)
    {
        if (billboard == null) return;

        billboards.Add(billboard);
    }

    public void Unregister(Billboard billboard)
    {
        if (billboard == null) return;

        billboards.Remove(billboard);
    }

    private void LateUpdate()
    {
        for (int i = billboards.Count - 1; i >= 0; i--) {
            var billboard = billboards[i];
            if (billboard == null) {
                billboards.RemoveAt(i);
                continue;
            }

            billboard.Tick();
        }
    }
}