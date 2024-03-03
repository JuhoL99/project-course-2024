using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damageable : MonoBehaviour
{
    private int health;
    [SerializeField] private int maxHealth = 20;
    EnemyNavigation navigationScript;
    [SerializeField] private Healthbar slider;

    // Start is called before the first frame update
    void Start()
    {
        navigationScript = GetComponent<EnemyNavigation>();
        health = maxHealth;
        slider.UpdateValue(1);
    }

    void Update()
    {
        // Kill cheat
        if (Input.GetKeyDown(KeyCode.K))
        {
            Destroy(gameObject);
        }
    }
    public void GetHitLoser(float knockbackAmount, int damage)
    {
        TakeDamage(damage);
        navigationScript.KnockBack(knockbackAmount);
    }
    private void TakeDamage(int damage)
    {
        health -= damage;
        HealthCheck();
    }
    private void HealthCheck()
    {
        slider.UpdateValue(health/maxHealth);
        if(health <= 0)
        {
            Destroy(gameObject);
            print("Destroyed");
        }
    }
}
