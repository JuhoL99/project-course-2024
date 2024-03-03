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

    // Update is called once per frame
    void Update()
    {
        // Kill test
        if (Input.GetKeyDown(KeyCode.K))
        {
            Destroy(gameObject);
        }

    }
    public void GetHitLoser(float knockbackAmount)
    {
        TakeDamage();
        navigationScript.KnockBack(knockbackAmount);
        //Destroy(gameObject);
    }
    private void TakeDamage()
    {
        health -= 5;
        HealthCheck();
    }
    private void HealthCheck()
    {
        slider.UpdateValue(health/maxHealth);
        if(health <= 0)
            Destroy(gameObject);
    }
}
