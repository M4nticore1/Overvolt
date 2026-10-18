using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    private static EnemiesManager instance;
    public static EnemiesManager Instance => instance ??= FindFirstObjectByType<EnemiesManager>();

    private List<Enemy> enemies = new();

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
        for (int i = enemies.Count - 1; i >= 0; i--) {
            var enemy = enemies[i];
            if (enemy == null) {
                enemies.RemoveAt(i);
                continue;
            }

            enemy.Tick();
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        if (enemy == null) return;

        if (!enemies.Contains(enemy)) {
            enemies.Add(enemy);
        }
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }
}