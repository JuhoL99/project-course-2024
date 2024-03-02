using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnPointEmpty;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Vector3[] spawnPoint;
    [SerializeField] private float spawnCircleRadius = 5f;
    [SerializeField] private int numOfEnemies = 5;

    private void Awake()
    {
        if(spawnPointEmpty == null)
        {
            spawnPointEmpty = this.gameObject;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnEnemy();
        }
    }
    private void GetSpawnPoints()
    {
        spawnPoint = new Vector3[numOfEnemies];
        spawnPoint[0] = spawnPointEmpty.transform.position;
        spawnPoint[0].y = spawnPointEmpty.transform.position.y;

        float minDistance = 1f;

        for (int i = 1; i < numOfEnemies; i++)
        {
            bool validPoint = false;

            while (!validPoint)
            {
                Vector2 randomPointInCircle = Random.insideUnitCircle * spawnCircleRadius;
                Vector3 candidatePoint = new Vector3(randomPointInCircle.x, spawnPointEmpty.transform.position.y, randomPointInCircle.y) + spawnPointEmpty.transform.position;
                candidatePoint.y = 0.5f;
                validPoint = true;

                foreach (Vector3 existingPoint in spawnPoint)
                {
                    if (Vector3.Distance(candidatePoint, existingPoint) < minDistance)
                    {
                        validPoint = false;
                        break;

                    }
                }
                if (validPoint)
                {
                    spawnPoint[i] = candidatePoint;
                }
            }
        }
    }
    public void SpawnEnemy()
    {
        GetSpawnPoints();
        for (int i = 0; i < numOfEnemies; i++)
        {
            Instantiate(enemyPrefab, spawnPoint[i], Quaternion.Euler(0, 0, 0));
        }
    }
}
