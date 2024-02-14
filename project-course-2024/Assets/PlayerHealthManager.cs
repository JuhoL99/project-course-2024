using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    public static PlayerHealthManager instance;
    private int maxHealth = 50;
    [SerializeField] private int currentHealth;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        HealthCheck();
    }
    private void HealthCheck()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("game over!");
        }
    }
}
