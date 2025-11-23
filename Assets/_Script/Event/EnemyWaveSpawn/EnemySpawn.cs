using System;
using UnityEngine;

[Serializable]
public class EnemySpawn
{
    public float timeStartSpawn = 0f;
    public GameObject enemyPrefab;
    public int totalSpawnCount;
    public int maxAliveCount;
    public float spawnInterval;

    [HideInInspector] public int spawnedCount = 0;
    [HideInInspector] public float lastSpawnTime = -Mathf.Infinity;
    [HideInInspector] public bool finished = false;
    [HideInInspector] public float waveStartTime = 0f;

    public bool CanSpawn(int currentAlive, float currentTime)
    {
        if (finished) return false;
        if (spawnedCount >= totalSpawnCount)
        {
            finished = true;
            return false;
        }
        if (currentAlive >= maxAliveCount) return false;

        float timeSinceWaveStart = currentTime - waveStartTime;

        if (timeSinceWaveStart < timeStartSpawn)
            return false;

        if (spawnedCount == 0)
            return timeSinceWaveStart >= timeStartSpawn;

        return timeSinceWaveStart >= (lastSpawnTime + spawnInterval - waveStartTime);
    }

    public void OnWaveStart(float waveStartTime)
    {
        this.waveStartTime = waveStartTime;
        lastSpawnTime = waveStartTime + timeStartSpawn - spawnInterval; // Để lần đầu spawn đúng lúc timeStartSpawn
        spawnedCount = 0;
        finished = false;
    }

    public void OnSpawned(float currentTime)
    {
        spawnedCount++;
        lastSpawnTime = currentTime;
        if (spawnedCount >= totalSpawnCount)
            finished = true;
    }
}
