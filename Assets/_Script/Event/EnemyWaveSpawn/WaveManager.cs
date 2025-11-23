using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public bool StopRun;
    public List<Transform> spawnPoints;
    public List<Wave> waves;
    public Transform enemyRootContainer; // Gán từ editor hoặc tự tạo ở Awake nếu cần

    private Dictionary<string, Transform> enemyContainers = new Dictionary<string, Transform>();
    private float waveTimer = 0f;
    private int currentWaveIndex = -1;
    private float currentWaveStartTime = 0f;
    void Start()
    {
        if (StopRun) return;
        ValidateWaves();
    }

    void Update()
    {
        if (StopRun) return;
        waveTimer += Time.deltaTime;

        if (currentWaveIndex + 1 < waves.Count)
        {
            Wave nextWave = waves[currentWaveIndex + 1];
            if (!nextWave.started && (waveTimer >= nextWave.startTime || (currentWaveIndex >= 0 && waves[currentWaveIndex].finished)))
            {
                StartNextWave();
            }
        }

        for (int i = 0; i <= currentWaveIndex; i++)
        {
            if (waves[i].started && !waves[i].finished)
            {
                HandleSpawning(waves[i]);
            }
        }

    }

    void StartNextWave()
    {
        currentWaveIndex++;
        Wave currentWave = waves[currentWaveIndex];
        currentWave.started = true;
        currentWaveStartTime = waveTimer;

        foreach (var spawnData in currentWave.spawns)
        {
            spawnData.OnWaveStart(currentWaveStartTime);
        }

        Debug.Log($"Wave {currentWaveIndex + 1} started!");
    }

    void HandleSpawning(Wave wave)
    {
        foreach (EnemySpawn spawnData in wave.spawns)
        {
            int aliveOfType = CountAliveOfType(spawnData.enemyPrefab);

            if (spawnData.CanSpawn(aliveOfType, waveTimer))
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
                Vector3 offset = Random.insideUnitCircle * 1.5f;
                Vector3 spawnPosition = spawnPoint.position + offset;

                // Get prefab name without (Clone)
                string prefabName = spawnData.enemyPrefab.name;

                // Get or create container
                Transform container = GetOrCreateContainer(prefabName);

                // Instantiate inside the container
                GameObject enemy = Instantiate(spawnData.enemyPrefab, spawnPosition, Quaternion.identity, container);

                spawnData.OnSpawned(waveTimer);
            }
        }

        if (wave.AllFinished())
        {
            wave.finished = true;
            Debug.Log($"Wave {currentWaveIndex + 1} finished!");
        }
    }

    Transform GetOrCreateContainer(string prefabName)
    {
        if (enemyContainers.ContainsKey(prefabName))
            return enemyContainers[prefabName];

        Transform container = enemyRootContainer.Find(prefabName);
        if (container == null)
        {
            GameObject newContainer = new GameObject(prefabName);
            newContainer.transform.parent = enemyRootContainer;
            container = newContainer.transform;
        }

        enemyContainers[prefabName] = container;
        return container;
    }

    int CountAliveOfType(GameObject prefab)
    {
        string prefabName = prefab.name;
        Transform container;
        if (!enemyContainers.TryGetValue(prefabName, out container))
        {
            container = enemyRootContainer.Find(prefabName);
            if (container == null) return 0;
        }

        int aliveCount = 0;
        foreach (Transform child in container)
        {
            if (child != null && child.gameObject.activeInHierarchy)
                aliveCount++;
        }
        return aliveCount;
    }

    void ValidateWaves()
    {
        bool hasCriticalError = false;

        for (int i = 0; i < waves.Count; i++)
        {
            Wave wave = waves[i];

            // Kiểm tra thời gian start tăng dần
            if (i > 0 && wave.startTime < waves[i - 1].startTime)
            {
                Debug.LogError($"❌ Wave {i + 1} có startTime ({wave.startTime}) nhỏ hơn wave trước ({waves[i - 1].startTime})");
                hasCriticalError = true;
            }

            for (int j = 0; j < wave.spawns.Count; j++)
            {
                EnemySpawn spawn = wave.spawns[j];

                if (spawn.enemyPrefab == null)
                {
                    Debug.LogError($"❌ Wave {i + 1} spawn {j + 1} không có prefab!");
                    hasCriticalError = true;
                }

                if (spawn.maxAliveCount > spawn.totalSpawnCount)
                {
                    Debug.LogError($"❌ Wave {i + 1}, prefab '{spawn.enemyPrefab.name}' có maxAliveCount ({spawn.maxAliveCount}) > totalSpawnCount ({spawn.totalSpawnCount})");
                    hasCriticalError = true;
                }
            }
        }

        if (hasCriticalError)
        {
#if UNITY_EDITOR
            Debug.Break(); // Pause game trong editor
#endif
        }
        else
        {
            Debug.Log("✅ Validate wave: không phát hiện lỗi.");
        }
    }
}
