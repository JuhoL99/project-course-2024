using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    static ResourceSpawner instance;
    public float spawnOutsideRadius, worldBorderRadius, spawnInterval;
    public int startResourceCount, maxResourceCount;
    public GameObject[] resourcePrefabs;

    float timer, radiusDiff;
    Transform resourceParent;
    LayerMask terrainLayer;
    float tau;
    public int worldResourceCount;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
        timer = spawnInterval;
        radiusDiff = worldBorderRadius - spawnOutsideRadius;
        resourceParent = GameObject.Find("Resources").transform;
        terrainLayer = 1 << LayerMask.NameToLayer("Terrain");
        tau = 2 * Mathf.PI;

        for (int i = 0; i < startResourceCount; i++)
        {
            SpawnRandomResource();
        }
        worldResourceCount = startResourceCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (worldResourceCount >= maxResourceCount) return;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = spawnInterval;
            SpawnRandomResource();
        }
    }
    void SpawnRandomResource()
    {
        int randIndex = Random.Range(0, resourcePrefabs.Length);
        float randomAngle = Random.Range(0, tau);
        Vector2 xz = new Vector2(Mathf.Sin(randomAngle), Mathf.Cos(randomAngle))
            * (spawnOutsideRadius + Random.Range(0, radiusDiff));
        print(xz);
        float terrainHeight;
        if (Physics.Raycast(new Vector3(xz.x, 100, xz.y), Vector3.down, out RaycastHit hitInfo, 200, terrainLayer))
        {
            terrainHeight = hitInfo.point.y;
        }
        else
        {
            print("Resource spawner can't find terrain height");
            return;
        }
        GameObject chosenPrefab = resourcePrefabs[randIndex];
        GameObject newResource = Instantiate(chosenPrefab,
            new Vector3(xz.x, terrainHeight + chosenPrefab.transform.localScale.y / 2, xz.y),
            Quaternion.Euler(0, Random.Range(0, 360f), 0), resourceParent.GetChild(randIndex));
        newResource.GetComponent<ResourceBeh>().spawnerScript = this;
        worldResourceCount++;

    }
}
