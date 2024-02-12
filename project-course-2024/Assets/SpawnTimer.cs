using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpawnTimer : MonoBehaviour
{
    private EnemySpawner spawner;
    private int enemiesToSpawn = 30;
    public WorldTime worldTime;
    private float spawnTimer;
    private float time = 0;

    [SerializeField] private float spawnRate;

    void Start()
    {
        spawner = GetComponent<EnemySpawner>();
        worldTime = WorldTime.instance;
        spawnRate = (worldTime.dayLength*12/24) / (enemiesToSpawn / 5);
        spawnTimer = spawnRate;
    }

    void Update()
    {
        if(worldTime.currentTimeOfDay == TimeOfDay.Night)
        {
            SpawnSequence();
        }
    }
    private void SpawnSequence()
    {
        time += Time.deltaTime;
        if(time > spawnTimer)
        {
            time = 0;
            spawner.SpawnEnemy();
            return;
        }
        
    }
}
