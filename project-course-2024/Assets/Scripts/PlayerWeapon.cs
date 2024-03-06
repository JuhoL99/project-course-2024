using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerWeapon : MonoBehaviour
{
    private Collider coll;
    [SerializeField] private float knockBackAmount;
    [SerializeField] private int weaponDamage;
    GameObject clawEffect;
    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<Collider>();
        coll.enabled = false;
        if (transform.childCount == 0) return;
        clawEffect = transform.GetChild(0).gameObject;
        clawEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EnableWeapon(bool value)
    {
        coll.enabled = value;
        if (clawEffect != null) clawEffect.SetActive(value);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Damageable enemyScript = other.GetComponent<Damageable>();
            if(enemyScript != null)
            {
                enemyScript.GetHitLoser(knockBackAmount, weaponDamage);
                Time.timeScale = 0.1f;
                Invoke("TimeScaleToNormal", 0.012f);
            }
        }
    }
    void TimeScaleToNormal()
    {
        Time.timeScale = 1f;
    }
}
