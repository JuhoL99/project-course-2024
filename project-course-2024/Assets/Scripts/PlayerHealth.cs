using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int maxHealth = 20;
    public float hotInterval; // hot = heal over time
    float hotTimer;
    [SerializeField] private Healthbar hpBar;
    private void Start()
    {
        health = maxHealth;
        hotTimer = hotInterval;
    }
    void Update()
    {
        HealOverTime();
        hpBar.UpdateValue(health / (float)maxHealth);
    }
    void HealOverTime()
    {
        hotTimer -= Time.deltaTime;
        if (hotTimer < 0)
        {
            hotTimer = hotInterval;
            ChangeHealth(1);
        }
    }
    public void GetHitLoser(int damage)
    {
        ChangeHealth(-damage);
    }
    void ChangeHealth(int amount)
    {
        health += amount;
        health = Mathf.Clamp(health,0,maxHealth);
        //Change UI
        if (health <= 0)
            Destroy(gameObject);
    }
}
