using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private Collider collider;
    [SerializeField] private float knockBackAmount;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        collider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EnableWeapon(bool value)
    {
        collider.enabled = value;
        Debug.Log("Works");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("triggered");
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("ENEMY TRIGGERED");
            Damageable enemyScript = other.GetComponent<Damageable>();
            if(enemyScript != null)
                enemyScript.GetHitLoser(knockBackAmount);
        }
    }

}
