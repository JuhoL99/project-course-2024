using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockOn : MonoBehaviour
{
    public float lockOnRange = 20f;
    [System.NonSerialized] public bool lockedOn;
    BuildAWall buildScript;
    Transform lockTarget;
    Transform cam;
    LockOnVolume lockOnVolume;
    Animator anim;
    CameraBeh cameraScript;
    float lockOffRange;
    public GameObject lockOnSpritePrefab;
    GameObject lockOnSprite;
    void Start()
    {
        buildScript = GetComponent<BuildAWall>();
        lockOnVolume = GetComponentInChildren<LockOnVolume>();
        cam = Camera.main.transform;
        cameraScript = GetComponent<CameraBeh>();
        lockOffRange = lockOnVolume.GetComponent<SphereCollider>().radius*1.4f;
        anim = GetComponent<Animator>();
        lockOnSprite = Instantiate(lockOnSpritePrefab);
        lockOnSprite.SetActive(false);
    }

    void Update()
    {
        LockedOnUpdate();
    }
    void LockedOnUpdate()
    {
        if (!lockedOn) return;
        if (lockTarget == null || (lockTarget.position - transform.position).magnitude > lockOffRange) DeactivateLockOn();
    }
    public void OnLockOnInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || buildScript.buildMode) return;
        if (lockedOn)
        {
            DeactivateLockOn();
        }
        else
        {
            ActivateLockOn();
        }
    }
    void ActivateLockOn()
    {
        GameObject target = FindClosestEnemyToReticle();
        if (target == null) return;
        lockedOn = true;
        anim.SetBool("LockedOn",true);
        cameraScript.ActivateLockOn(target.transform);
        lockTarget = target.transform;
        lockOnSprite = Instantiate(lockOnSpritePrefab, lockTarget);
    }
    void DeactivateLockOn()
    {
        cameraScript.DeactivateLockOn();
        lockedOn = false;
        anim.SetBool("LockedOn", false);
        Destroy(lockOnSprite);
        lockTarget = null;
    }
    GameObject FindClosestEnemyToReticle()
    {
        float smallestAngle = 90f;
        GameObject chosenEnemy = null;
        GameObject[] enemiesInVolume = lockOnVolume.enemiesInVolume.ToArray();
        foreach (GameObject go in enemiesInVolume)
        {
            if (go == null)
            {
                lockOnVolume.enemiesInVolume.Remove(go); 
                continue;
            }
            float enemyCamAngle = Vector3.Angle(go.transform.position - cam.position, cam.forward);
            if (enemyCamAngle < smallestAngle)
            {
                smallestAngle = enemyCamAngle;
                chosenEnemy = go;
            }
        }
        return chosenEnemy;
    }
}
