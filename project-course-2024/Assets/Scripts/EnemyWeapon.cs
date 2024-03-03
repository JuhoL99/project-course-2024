using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
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
            DamageablePlayer dmg = other.gameObject.GetComponent<DamageablePlayer>();
            dmg.GetHitLoser();
            Debug.Log("Hit player");
        }
    }
}
