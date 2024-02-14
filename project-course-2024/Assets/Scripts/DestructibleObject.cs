using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private NavMeshManager navMeshManager;
    void Start()
    {
        currentHealth = maxHealth;
        navMeshManager = NavMeshManager.instance;
    }
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        HealthCheck();
    }
    private void HealthCheck()
    {
        if(currentHealth <= 0)
        {
            gameObject.SetActive(false);
            //navMeshManager.RebakeNavMesh();
            Destroy(gameObject);
        }
    }
}
