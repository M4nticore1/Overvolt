using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EndlessWavesEnemyData
{
    public Enemy enemyPrefab;
    public int cost;
    public int minCount;
    public int maxCount;
}

public class EndlessWavesManager : MonoBehaviour
{
    [SerializeField] private EndlessWavesEnemyData[] enemies;

    [Header("Spawn Credits")]
    [SerializeField] private float spawnCreditsIncrease = 1f;
    [SerializeField] private float spawnCreditsAcceleration = 0.01f;
    [SerializeField] private float currentSpawnCredits = 0f;

    [Header("Spawn")]
    [SerializeField] private SpawnArea[] spawnAreas;
    [SerializeField] private float spawnInterval = 1f;
    private float spawnTimer;

    private void Update()
    {
        currentSpawnCredits += spawnCreditsIncrease * Time.deltaTime;
        spawnCreditsIncrease += spawnCreditsAcceleration * Time.deltaTime;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval) {
            spawnTimer = 0f;
            TrySpawnRandomEnemy();
        }
    }

    private void TrySpawnRandomEnemy()
    {
        if (spawnAreas == null || spawnAreas.Length == 0) return;

        var availableWaveEnemiesData = GetAvailableWaveEnemiesData();
        if (availableWaveEnemiesData.Count == 0) return;

        var waveEnemyDataIndex = UnityEngine.Random.Range(0, availableWaveEnemiesData.Count);
        var waveEnemyData = availableWaveEnemiesData[waveEnemyDataIndex];

        var enemiesCount = GetRandomEnemiesCount(waveEnemyData);
        if (enemiesCount <= 0) return;

        int totalCost = enemiesCount * waveEnemyData.cost;
        currentSpawnCredits -= totalCost;

        var spawnArea = GetRandomSpawnArea();
        var enemyData = EnemyData.Default();

        for (int i = 0; i < enemiesCount; i++) {
            Enemy.Create(waveEnemyData.enemyPrefab, spawnArea.GetRandomSpawnPosition(), spawnArea.GetSpawnRotation(), enemyData);
        }
    }

    private int GetRandomEnemiesCount(EndlessWavesEnemyData waveEnemyData)
    {
        var maxAffordable = Mathf.FloorToInt((float)currentSpawnCredits / waveEnemyData.cost);
        if (maxAffordable <= 0) return 0;

        var maxCount = Mathf.Min(waveEnemyData.maxCount, maxAffordable);
        if (waveEnemyData.minCount > maxCount) return 0;

        return UnityEngine.Random.Range(waveEnemyData.minCount, maxCount + 1);
    }

    private List<EndlessWavesEnemyData> GetAvailableWaveEnemiesData()
    {
        var availableWaveEnemiesData = new List<EndlessWavesEnemyData>();
        int intCredits = Mathf.FloorToInt(currentSpawnCredits);

        for (int i = 0; i < enemies.Length; i++) {
            var enemyData = enemies[i];
            if (enemyData.cost > intCredits) continue;

            availableWaveEnemiesData.Add(enemyData);
        }

        return availableWaveEnemiesData;
    }

    private SpawnArea GetRandomSpawnArea()
    {
        var index = UnityEngine.Random.Range(0, spawnAreas.Length);

        return spawnAreas[index];
    }
}