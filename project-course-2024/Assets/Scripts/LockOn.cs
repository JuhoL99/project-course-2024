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
    void Start()
    {
        buildScript = GetComponent<BuildAWall>();
        lockOnVolume = transform.GetChild(1).GetComponent<LockOnVolume>();
        cam = Camera.main.transform;
        cameraScript = GetComponent<CameraBeh>();
    }

    void Update()
    {
        LockedCameraUpdate();
    }
    void LockedCameraUpdate()
    {
        if (!lockedOn) return;
    }
    public void OnLockOnInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || buildScript.buildMode) return;
        lockedOn = !lockedOn;
        if (lockedOn) DeactivateLockOn(); else ActivateLockOn();
    }
    void ActivateLockOn()
    {
        GameObject target = FindClosestEnemyToReticle();
        if (target == null) return;
        cameraScript.ActivateLockOn(target.transform);


    }
    void DeactivateLockOn()
    {
        cameraScript.DeactivateLockOn();
    }
    GameObject FindClosestEnemyToReticle()
    {
        float smallestAngle = 90f;
        GameObject chosenEnemy = null;
        foreach (GameObject go in lockOnVolume.enemiesInVolume)
        {
            float enemyCamAngle = Vector3.Angle(go.transform.position - cam.position, cam.forward);

            if (enemyCamAngle > smallestAngle)
            {
                smallestAngle = enemyCamAngle;
                chosenEnemy = go;
            }
        }
        return chosenEnemy;
    }
    
}
