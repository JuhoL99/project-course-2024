using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraBeh : MonoBehaviour
{
    CinemachineFreeLook freeLook;
    [System.NonSerialized] public bool buildMode, lockOn;
    bool yLerping;
    float camOrigYSpeed, camOrigXSpeed, lerpTarget;
    Transform lockOnTarget, cam;
    Movement moveScript;
    void Start()
    {
        freeLook = GameObject.Find("FreeLook Camera").GetComponent<CinemachineFreeLook>();
        cam = Camera.main.transform;
        camOrigYSpeed = freeLook.m_YAxis.m_MaxSpeed;
        camOrigXSpeed = freeLook.m_XAxis.m_MaxSpeed;
        moveScript = GetComponent<Movement>();
    }

    void Update()
    {
        
    }
    void LateUpdate()
    {
        CamYLerp();
        LockOnUpdate();
    }
    void CamYLerp()
    {
        if (yLerping)
        {
            float yAxis = freeLook.m_YAxis.Value;
            freeLook.m_YAxis.Value = Mathf.Lerp(yAxis, lerpTarget, 6f * Time.deltaTime);
            if (Mathf.Abs(lerpTarget - yAxis) < 0.01f)
            {
                yLerping = false;
            }
        }
    }
    void LockOnUpdate()
    {
        if (!lockOn || lockOnTarget == null) return;
        Vector3 playerToEnemy = lockOnTarget.position - transform.position;
        freeLook.m_XAxis.Value = Mathf.Rad2Deg * Mathf.Atan2(playerToEnemy.x, playerToEnemy.z);
            //+ Vector2.Dot(moveScript.moveDir2.normalized, new Vector2(cam.forward.z,cam.forward.x).normalized) * 30f;
    }
    public void SetBuildCameraEnabled(bool enabled)
    {
        buildMode = enabled;
        if (enabled)
        {
            freeLook.m_YAxis.m_MaxSpeed = 0;
            freeLook.m_XAxis.m_MaxSpeed = 0;
            lerpTarget = 1f;
            yLerping = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            freeLook.m_YAxis.m_MaxSpeed = camOrigYSpeed;
            freeLook.m_XAxis.m_MaxSpeed = camOrigXSpeed;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void ActivateLockOn(Transform target)
    {
        lockOn = true;
        lockOnTarget = target;
        freeLook.LookAt = target;
        freeLook.m_YAxis.m_MaxSpeed = 0;
        freeLook.m_XAxis.m_MaxSpeed = 0;
        lerpTarget = 0.4f;
        yLerping = true;
    }
    public void DeactivateLockOn()
    {
        lockOn = false;
        lockOnTarget = null;
        freeLook.m_YAxis.m_MaxSpeed = camOrigYSpeed;
        freeLook.m_XAxis.m_MaxSpeed = camOrigXSpeed;
        freeLook.LookAt = transform;
    }
}
