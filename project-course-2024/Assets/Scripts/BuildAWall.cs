using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildAWall : MonoBehaviour
{
    public GameObject[] buildingBlocks;
    public GameObject ghostBlockTemplate;
    public Material buildMaterial;
    public int gridSize;
    GameObject ghostObject;
    MeshFilter ghostMeshFilter;
    bool buildMode, cameraBuildModeLerp;
    int chosenBuildingBlockIndex, buildRotationInt;
    int buildingListLength;
    float camOrigYSpeed, camOrigXSpeed;
    Vector2Int buildCoords;
    List<Vector2Int> builtCoords = new List<Vector2Int>();
    CinemachineFreeLook freeLook;
    Quaternion buildRotation;
    void Start()
    {
        buildingListLength = buildingBlocks.Length;
        freeLook = GameObject.Find("FreeLook Camera").GetComponent<CinemachineFreeLook>();
        camOrigYSpeed = freeLook.m_YAxis.m_MaxSpeed;
        camOrigXSpeed = freeLook.m_XAxis.m_MaxSpeed;
    }

    void Update()
    {
        UpdateGhostObjectPos();
    }
    void LateUpdate()
    {
        BuildModeCameraUpdate();
    }
    void BuildModeCameraUpdate()
    {
        if (buildMode && cameraBuildModeLerp)
        {
            float yAxis = freeLook.m_YAxis.Value;
            freeLook.m_YAxis.Value = Mathf.Lerp(yAxis, 1f, 0.1f);
            if (1 - yAxis < 0.001f)
            {
                cameraBuildModeLerp = false;
            }
        }
    }
    void UpdateGhostObjectPos()
    {
        if (!buildMode) return;
        Vector3 pointingPos = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            pointingPos = hitInfo.point;
        }
        buildCoords = new Vector2Int(
        Mathf.FloorToInt(pointingPos.x / gridSize),
        Mathf.FloorToInt(pointingPos.z / gridSize)) * gridSize;
        Vector3 worldBuildPos = new Vector3(buildCoords.x + gridSize / 2f,
            ghostObject.transform.localScale.y / 2 + 0.01f,
            buildCoords.y + gridSize / 2f);

        //buildRot.y = Mathf.FloorToInt((buildRot.y + 45) / 90f) * 90f;
        ghostObject.transform.position = worldBuildPos;
    }
    void SetBuildCameraEnabled(bool enabled)
    {
        if (enabled)
        {
            freeLook.m_YAxis.m_MaxSpeed = 0;
            freeLook.m_XAxis.m_MaxSpeed = 0;
            cameraBuildModeLerp = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            freeLook.m_YAxis.m_MaxSpeed = camOrigYSpeed;
            freeLook.m_XAxis.m_MaxSpeed = camOrigXSpeed;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void OnBuildInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (buildMode) ExitBuildMode(); else EnterBuildMode();
    }
    public void OnFireInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (buildMode)
        {
            BuildObject();
        }
    }
    public void OnCycleBuildingBlocks(InputAction.CallbackContext ctx)
    {
        if (!(ctx.performed && buildMode)) return;
        chosenBuildingBlockIndex += (ctx.ReadValue<float>() > 0 ? 1 : -1) + buildingListLength;
        chosenBuildingBlockIndex %= buildingListLength;
        SetGhostObjectMesh();
    }
    public void OnCycleBuildingRotation(InputAction.CallbackContext ctx)
    {
        if (!(ctx.performed && buildMode)) return;
        buildRotationInt += (ctx.ReadValue<float>() > 0 ? 1 : -1) + 4;
        buildRotationInt %= 4;
        buildRotation = Quaternion.Euler(0, buildRotationInt * 90f, 0);
        ghostObject.transform.rotation = buildRotation;
    }
    void EnterBuildMode()
    {
        SetBuildCameraEnabled(true);
        buildMode = true;
        ghostObject = Instantiate(ghostBlockTemplate);
        ghostMeshFilter = ghostObject.GetComponent<MeshFilter>();
        SetGhostObjectMesh();
    }
    void SetGhostObjectMesh()
    {
        GameObject chosenPrefab = buildingBlocks[chosenBuildingBlockIndex];
        ghostMeshFilter.sharedMesh = chosenPrefab.GetComponent<MeshFilter>().sharedMesh;
        ghostObject.transform.localScale = chosenPrefab.transform.localScale;
    }
    void ExitBuildMode()
    {
        SetBuildCameraEnabled(false);
        buildMode = false;
        Destroy(ghostObject);
        ghostObject = null;
    }
    void BuildObject()
    {
        GameObject newObject = Instantiate(buildingBlocks[chosenBuildingBlockIndex], 
            ghostObject.transform.position, ghostObject.transform.rotation);
        builtCoords.Add(buildCoords);
    }
}
