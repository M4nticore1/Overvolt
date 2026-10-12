using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    private List<Enemy> enemies = new();

    //private void Update()
    //{
    //    for (int i = enemies.Count - 1; i >= 0; i--) {
    //        var enemy = enemies[i];
    //        if (enemy == null) {
    //            enemies.RemoveAt(i);
    //            continue;
    //        }

    //        enemy.Tick();
    //    }
    //}

    public void RegisterEnemy(Enemy enemy)
    {
        if (enemy == null) return;

        if (!enemies.Contains(enemy)) {
            enemies.Add(enemy);
        }
    }

    public void UnregisterManager(Enemy enemy)
    {
        enemies.Remove(enemy);
    }
}