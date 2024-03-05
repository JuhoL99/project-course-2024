using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public int enemyAttackDamage;
    private Collider wCollider;

    void Start()
    {
        wCollider = GetComponent<Collider>();
        wCollider.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            wCollider.enabled = false;
            PlayerHealth playerHealthScript = other.gameObject.GetComponent<PlayerHealth>();
            playerHealthScript.GetHitLoser(enemyAttackDamage);
        }
    }
}
