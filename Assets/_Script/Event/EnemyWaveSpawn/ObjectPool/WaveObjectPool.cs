using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    public CreateObject createObject;    // prefab + quantity
    public float startSpawn;             // thời gian delay trong wave
    public float spawnInterval;          // khoảng cách spawn
    public int maxAlive;                 // số enemy tối đa tồn tại cùng lúc

    [HideInInspector] public int spawnedCount;
    [HideInInspector] public float nextSpawnTime;
    [HideInInspector] public bool finished;
}

[System.Serializable]
public class WaveData
{
    public float startTime;
    public bool started;
    public bool finished;
    public List<EnemySpawnData> enemySpawns;
}

public class WaveObjectPool : MonoBehaviour
{
    public bool systemActive = true;
    public List<Transform> spawnPoints;
    public List<WaveData> waves;
    public List<CreateObject> allObjects = new List<CreateObject>();
    private int currentWaveIndex = 0;
    private float gameTime = 0f;

    private void Awake()
    {
        CreateEnemyPools();
    }
    void Update()
    {
        if (!systemActive || waves.Count == 0) return;

        gameTime += Time.deltaTime;

        for (int i = currentWaveIndex; i < waves.Count; i++)
        {
            WaveData wave = waves[i];

            // Điều kiện start wave
            if (!wave.started)
            {
                bool prevFinished = (i == 0 || waves[i - 1].finished);
                if ((gameTime >= wave.startTime) || prevFinished)
                {
                    StartCoroutine(RunWave(wave, i));
                    wave.started = true;
                    currentWaveIndex = i;
                }
            }
        }
    }
    void CreateEnemyPools()
    {
        

        foreach (var wave in waves)
        {
            foreach (var spawn in wave.enemySpawns)
            {
                allObjects.Add(spawn.createObject);
            }
        }

        ObjectPoolManager.Instance.CreateObjects(allObjects);
    }

    private IEnumerator RunWave(WaveData wave, int waveIndex)
    {
        Debug.Log($"Wave {waveIndex + 1} started!");

        // Reset trạng thái enemySpawn
        foreach (var spawn in wave.enemySpawns)
        {
            spawn.spawnedCount = 0;
            spawn.finished = false;
            spawn.nextSpawnTime = gameTime + spawn.startSpawn;
        }

        // Quản lý vòng đời wave
        while (true)
        {
            bool allFinished = true;

            foreach (var spawn in wave.enemySpawns)
            {
                if (spawn.finished) continue;

                // Check số lượng spawn
                if (spawn.spawnedCount >= spawn.createObject.quantity)
                {
                    spawn.finished = true;
                    continue;
                }

                // Check maxAlive trong scene
                int alive = CountAlive(spawn);
                if (alive >= spawn.maxAlive)
                {
                    allFinished = false;
                    continue;
                }

                // Đủ điều kiện spawn
                if (gameTime >= spawn.nextSpawnTime)
                {
                    Transform point = spawnPoints[Random.Range(0, spawnPoints.Count)];
                    GameObject obj = ObjectPoolManager.Instance.GetPool(
                        spawn.createObject.gameObject.tag,
                        GetObjectName(spawn.createObject.gameObject),
                        point.position
                    );

                    if (obj != null)
                    {
                        spawn.spawnedCount++;
                        spawn.nextSpawnTime = gameTime + spawn.spawnInterval;
                    }
                }

                allFinished = false;
            }

            if (allFinished) break;
            yield return null;
        }

        wave.finished = true;
        Debug.Log($"Wave {waveIndex + 1} finished!");
    }

    private int CountAlive(EnemySpawnData spawn)
    {
        string tagName = spawn.createObject.gameObject.tag;
        string objectName = GetObjectName(spawn.createObject.gameObject);
        string key = tagName + "_" + objectName;

        if (!ObjectPoolManager.Instance.HasContainer(key)) return 0;

        int alive = 0;
        Transform container = ObjectPoolManager.Instance.GetContainer(key);
        foreach (Transform child in container)
        {
            if (child.gameObject.activeInHierarchy) alive++;
        }
        return alive;
    }

    private string GetObjectName(GameObject prefab)
    {
        // Enemy
        //EnemyStats enemyStats = prefab.GetComponent<EnemyStats>();
        //if (enemyStats != null && enemyStats.enemyInformation != null)
        //    return enemyStats.enemyInformation.Name;

        return prefab.name;
    }
}
