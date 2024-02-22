using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBeh : MonoBehaviour
{
    CinemachineFreeLook freeLook;
    [System.NonSerialized] public bool buildMode, lockOn;
    bool buildModeLerp;
    float camOrigYSpeed, camOrigXSpeed;
    Transform lockOnTarget;

    void Start()
    {
        freeLook = GameObject.Find("FreeLook Camera").GetComponent<CinemachineFreeLook>();
        camOrigYSpeed = freeLook.m_YAxis.m_MaxSpeed;
        camOrigXSpeed = freeLook.m_XAxis.m_MaxSpeed;
    }

    void Update()
    {
        
    }
    void LateUpdate()
    {
        BuildModeCameraUpdate();
    }
    void BuildModeCameraUpdate()
    {
        if (buildMode && buildModeLerp)
        {
            float yAxis = freeLook.m_YAxis.Value;
            freeLook.m_YAxis.Value = Mathf.Lerp(yAxis, 1f, 6f * Time.deltaTime);
            if (1 - yAxis < 0.001f)
            {
                buildModeLerp = false;
            }
        }
    }
    public void SetBuildCameraEnabled(bool enabled)
    {
        if (enabled)
        {
            freeLook.m_YAxis.m_MaxSpeed = 0;
            freeLook.m_XAxis.m_MaxSpeed = 0;
            buildModeLerp = true;
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
    }
    public void DeactivateLockOn()
    {
        lockOn = false;
        lockOnTarget = null;
    }
}