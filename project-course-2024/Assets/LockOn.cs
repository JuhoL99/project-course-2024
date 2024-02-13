using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockOn : MonoBehaviour
{
    public float lockOnRange = 20f;
    bool lockedOn;
    BuildAWall buildScript;
    Transform lockTarget;
    Transform cam;
    LockOnVolume lockOnVolume;
    void Start()
    {
        buildScript = GetComponent<BuildAWall>();
        lockOnVolume = transform.GetChild(1).GetComponent<LockOnVolume>();
        cam = Camera.main.transform;
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
        if (!ctx.performed) return;
        lockedOn = !lockedOn;
        if (lockedOn) DeactivateLockOn(); else ActivateLockOn();
    }
    void ActivateLockOn()
    {
        float smallestAngle = 90f;
        foreach (GameObject go in lockOnVolume.enemiesInVolume)
        {
            float enemyCamAngle = Vector3.Angle(go.transform.position - cam.position, cam.forward)
            
            if (enemyCamAngle>smallestAngle)
            {

            }
        }
    }
    void DeactivateLockOn()
    {
        lockTarget = null;
    }
}
