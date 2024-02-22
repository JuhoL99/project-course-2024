using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildAWall : MonoBehaviour
{
    public Vector2Int buildAreaDimensions, buildAreaCenter;
    public float buildHeight;
    public GameObject[] buildingBlocks;
    public GameObject ghostBlockTemplate;
    public Material buildMaterial;
    public int gridSize;
    public GameObject[,] buildMatrix;
    public int[,] prefabIndexMatrix;
    GameObject ghostObject;
    MeshFilter ghostMeshFilter;
    [System.NonSerialized] public bool buildMode, destroyMode;
    int chosenBuildingBlockIndex, buildRotationInt;
    int buildingListLength;
    Vector2Int gridCoords, gridCoordsLastFrame;
    Quaternion buildRotation;
    LockOn lockOnScript;
    CameraBeh cameraScript;
    Vector3 pointingPos;
    void Start()
    {
        buildingListLength = buildingBlocks.Length;
        lockOnScript = GetComponent<LockOn>();
        cameraScript = GetComponent<CameraBeh>();
        buildMatrix = new GameObject[buildAreaDimensions[0], buildAreaDimensions[1]];
        for (int i = 0; i < buildAreaDimensions[0]; i++)
        {
            for (int j = 0; j < buildAreaDimensions[1]; j++)
            {
                buildMatrix[i, j] = null;
                prefabIndexMatrix[i, j] = -1;
            }
        }
        prefabIndexMatrix = new int[buildAreaDimensions[0], buildAreaDimensions[1]];
    }

    void Update()
    {
        BuildModeUpdate();
    }
    void BuildModeUpdate()
    {
        if (!buildMode) return;
        pointingPos = Vector3.zero;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            pointingPos = hitInfo.point;
        }
        gridCoords = new Vector2Int(
        Mathf.FloorToInt(pointingPos.x / gridSize),
        Mathf.FloorToInt(pointingPos.z / gridSize)) * gridSize;

        if (!gridCoords.Equals(gridCoordsLastFrame))
        {
            Vector3 worldBuildPos = new Vector3(gridCoords.x + gridSize / 2f,
            ghostObject.transform.localScale.y / 2 + buildHeight + 0.01f,
            gridCoords.y + gridSize / 2f);
            ghostObject.transform.position = worldBuildPos;
            if (destroyMode)
            {
                int index = prefabIndexMatrix[gridCoords.x, gridCoords.y];
                if (index >= 0)
                {
                    SetGhostObjectMesh(index);
                }
                else
                {
                    ClearGhostObjectMesh();
                }
            }
        }
        gridCoordsLastFrame = gridCoords;
    }
    
    public void OnBuildInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || lockOnScript.lockedOn) return;
        if (buildMode) ExitBuildMode(); else EnterBuildMode();
    }
    public void OnDestroyModeInput(InputAction.CallbackContext ctx)
    {
        if (!(ctx.performed && buildMode)) return;
        destroyMode = !destroyMode;
    }
    public void OnFireInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (buildMode) { if (destroyMode) DestroyObject(); else BuildObject(); }
    }
    public void OnCycleBuildingBlocks(InputAction.CallbackContext ctx)
    {
        if (!(ctx.performed && buildMode)) return;
        chosenBuildingBlockIndex += (ctx.ReadValue<float>() > 0 ? 1 : -1) + buildingListLength;
        chosenBuildingBlockIndex %= buildingListLength;
        SetGhostObjectMesh(chosenBuildingBlockIndex);
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
        SetGhostObjectMesh(chosenBuildingBlockIndex);
    }
    void ExitBuildMode()
    {
        cameraScript.SetBuildCameraEnabled(false);
        buildMode = false;
        destroyMode = false;
        Destroy(ghostObject);
        ghostObject = null;
    }
    void SetGhostObjectMesh(int prefabIndex)
    {
        GameObject chosenPrefab = buildingBlocks[prefabIndex];
        ghostMeshFilter.sharedMesh = chosenPrefab.GetComponent<MeshFilter>().sharedMesh;
        ghostObject.transform.localScale = chosenPrefab.transform.localScale;
    }
    void ClearGhostObjectMesh()
    {
        //ghostMeshFilter.sharedMesh = 
    }
    void BuildObject()
    {
        GameObject newObject = Instantiate(buildingBlocks[chosenBuildingBlockIndex], 
            ghostObject.transform.position-Vector3.up*0.01f, ghostObject.transform.rotation);
        buildMatrix[gridCoords[0], gridCoords[1]] = newObject;
        prefabIndexMatrix[gridCoords[0], gridCoords[1]] = chosenBuildingBlockIndex;
    }
    void DestroyObject()
    {
        Destroy(buildMatrix[gridCoords.x,gridCoords.y]);
        buildMatrix[gridCoords.x, gridCoords.y] = null;
    }
}
