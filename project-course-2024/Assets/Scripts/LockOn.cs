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
    CameraBeh cameraScript;
    float lockOffRange;
    void Start()
    {
        buildScript = GetComponent<BuildAWall>();
        lockOnVolume = transform.GetChild(1).GetComponent<LockOnVolume>();
        cam = Camera.main.transform;
        cameraScript = GetComponent<CameraBeh>();
        lockOffRange = lockOnVolume.GetComponent<SphereCollider>().radius*1.4f;
    }

    void Update()
    {
        LockedOnUpdate();
    }
    void LockedOnUpdate()
    {
        if (lockTarget == null || (lockTarget.position - transform.position).magnitude < lockOffRange) DeactivateLockOn();
    }
    public void OnLockOnInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || buildScript.buildMode) return;
        print(lockedOn);
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
        print(target.name);
        cameraScript.ActivateLockOn(target.transform);
        lockTarget = target.transform;

    }
    void DeactivateLockOn()
    {
        cameraScript.DeactivateLockOn();
        lockedOn = false;
    }
    GameObject FindClosestEnemyToReticle()
    {
        float smallestAngle = 90f;
        GameObject chosenEnemy = null;
        foreach (GameObject go in lockOnVolume.enemiesInVolume)
        {
            float enemyCamAngle = Vector3.Angle(go.transform.position - cam.position, cam.forward);
            print(enemyCamAngle);
            if (enemyCamAngle < smallestAngle)
            {
                print("smaller");
                smallestAngle = enemyCamAngle;
                chosenEnemy = go;
            }
        }
        return chosenEnemy;
    }
}
