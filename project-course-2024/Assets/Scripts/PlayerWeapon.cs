using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private Collider coll;
    [SerializeField] private float knockBackAmount;
    [SerializeField] private int weaponDamage;
    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider>();
        coll.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EnableWeapon(bool value)
    {
        coll.enabled = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Damageable enemyScript = other.GetComponent<Damageable>();
            if(enemyScript != null)
                enemyScript.GetHitLoser(knockBackAmount, weaponDamage);
        }
    }

}
