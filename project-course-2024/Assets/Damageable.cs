using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    private int health;
    [SerializeField] private int maxHealth = 20;
    EnemyNavigation navigationScript;
    // Start is called before the first frame update
    void Start()
    {
        navigationScript = GetComponent<EnemyNavigation>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GetHitLoser()
    {
        TakeDamage();
        navigationScript.KnockBack();
        //Destroy(gameObject);
    }
    private void TakeDamage()
    {
        health -= 5;
        HealthCheck();
    }
    private void HealthCheck()
    {
        if(health <= 0)
            Destroy(gameObject);
    }
}
