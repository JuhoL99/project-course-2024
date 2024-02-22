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
    [System.NonSerialized] public bool buildMode;
    int chosenBuildingBlockIndex, buildRotationInt;
    int buildingListLength;
    Vector2Int buildCoords;
    List<Vector2Int> builtCoords = new List<Vector2Int>();
    CinemachineFreeLook freeLook;
    Quaternion buildRotation;
    LockOn lockOnScript;
    CameraBeh cameraScript;
    void Start()
    {
        buildingListLength = buildingBlocks.Length;
        freeLook = GameObject.Find("FreeLook Camera").GetComponent<CinemachineFreeLook>();
        lockOnScript = GetComponent<LockOn>();
        cameraScript = GetComponent<CameraBeh>();
    }

    void Update()
    {
        UpdateGhostObjectPos();
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
    
    public void OnBuildInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || lockOnScript.lockedOn) return;
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
        cameraScript.SetBuildCameraEnabled(true);
        buildMode = true;
        ghostObject = Instantiate(ghostBlockTemplate);
        ghostMeshFilter = ghostObject.GetComponent<MeshFilter>();
        SetGhostObjectMesh();
    }
    void ExitBuildMode()
    {
        cameraScript.SetBuildCameraEnabled(false);
        buildMode = false;
        Destroy(ghostObject);
        ghostObject = null;
    }
    void SetGhostObjectMesh()
    {
        GameObject chosenPrefab = buildingBlocks[chosenBuildingBlockIndex];
        ghostMeshFilter.sharedMesh = chosenPrefab.GetComponent<MeshFilter>().sharedMesh;
        ghostObject.transform.localScale = chosenPrefab.transform.localScale;
    }
    void BuildObject()
    {
        GameObject newObject = Instantiate(buildingBlocks[chosenBuildingBlockIndex], 
            ghostObject.transform.position, ghostObject.transform.rotation);
        builtCoords.Add(buildCoords);
    }
}
