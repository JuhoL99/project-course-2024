using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageablePlayer : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int maxHealth = 20;
    private void Start()
    {
        health = maxHealth;
    }
    public void GetHitLoser()
    {
        TakeDamage();
    }
    private void TakeDamage()
    {
        health -= 2;
        HealthCheck();
    }
    private void HealthCheck()
    {
        if (health <= 0)
            Destroy(gameObject);
    }
}
