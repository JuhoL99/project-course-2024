using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildAWall : MonoBehaviour
{
    public Vector2Int buildAreaDimensions, buildAreaCenter;
    Vector2Int halfArea;
    public float buildHeight;
    public GameObject[] buildingBlocks;
    public GameObject ghostBlockTemplate;
    public int cellSize;
    public GameObject[,] builtObjectsMatrix;
    public int[,] prefabIndexMatrix;
    GameObject ghostObject;
    public Material[] ghostObjectMaterials = new Material[3];
    MeshFilter ghostMeshFilter;
    Renderer ghostRenderer;
    [System.NonSerialized] public bool buildMode, destroyMode;
    int chosenBuildingBlockIndex, buildRotationInt;
    int buildingListLength;
    Vector2Int square, squareIndex;
    Quaternion buildRotation;
    LockOn lockOnScript;
    CameraBeh cameraScript;
    Vector3 pointingPos;
    LayerMask destroyRaycastLayer, terrainLayer;
    bool canBuildHere = true;
    void Start()
    {
        halfArea = buildAreaDimensions / 2;
        buildingListLength = buildingBlocks.Length;
        lockOnScript = GetComponent<LockOn>();
        cameraScript = GetComponent<CameraBeh>();
        builtObjectsMatrix = new GameObject[buildAreaDimensions.x, buildAreaDimensions.y];
        prefabIndexMatrix = new int[buildAreaDimensions.x, buildAreaDimensions.y];
        for (int i = 0; i < buildAreaDimensions.x; i++)
        {
            for (int j = 0; j < buildAreaDimensions.y; j++)
            {
                builtObjectsMatrix[i, j] = null;
                prefabIndexMatrix[i, j] = -1;
            }
        }
        destroyRaycastLayer = LayerMask.GetMask(new string[]{"BuildingBlock","DestructibleObject"});
        terrainLayer = 1<<LayerMask.NameToLayer("Terrain");
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
        if (destroyMode)
        {
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 20f, destroyRaycastLayer))
            {
                pointingPos = hitInfo.transform.position;
                ghostObject.transform.rotation = hitInfo.transform.rotation;
            }
        } else
        {
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 20f, terrainLayer))
            {
                pointingPos = hitInfo.point;
            }
        }
        Vector2Int squareTemp = new Vector2Int(
        Mathf.FloorToInt(pointingPos.x / cellSize),
        Mathf.FloorToInt(pointingPos.z / cellSize));

        if (square.x != squareTemp.x || square.y != squareTemp.y)
        {
            square = squareTemp;


            SwitchSquare(false);
        }
        square = squareTemp;
    }
    void SwitchSquare(bool enter)
    {
        squareIndex = square + halfArea;
        bool inArea = squareInArea();
        bool canBuildHereTemp = inArea && builtObjectsMatrix[squareIndex.x, squareIndex.y] == null;
        bool canBuildSwitched = canBuildHere != canBuildHereTemp;
        canBuildHere = canBuildHereTemp;
        Vector3 worldBuildPos = new Vector3(square.x*cellSize + cellSize / 2f,
        ghostObject.transform.localScale.y / 2 + buildHeight + 0.01f,
        square.y*cellSize + cellSize / 2f);
        ghostObject.transform.position = worldBuildPos;
        if (destroyMode)
        {
            if (inArea)
            {
                int index = prefabIndexMatrix[squareIndex.x, squareIndex.y];
                if (index >= 0)
                {
                    SetGhostObjectMesh(index);
                }
                else
                {
                    ClearGhostObjectMesh();
                }
            }
            else
            {
                ClearGhostObjectMesh();
            }
        }
        else if (canBuildSwitched || enter)
        {
            ghostRenderer.material = ghostObjectMaterials[Convert.ToInt32(!canBuildHere)];
        }
    }
    
    public void OnBuildInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || lockOnScript.lockedOn) return;
        if (buildMode) { ExitBuildMode(); } else { EnterBuildMode(); }
    }
    public void OnDestroyModeInput(InputAction.CallbackContext ctx)
    {
        if (!(ctx.performed && buildMode)) return;
        destroyMode = !destroyMode;
        print(destroyMode);
        SwitchDestroyMode(destroyMode);
    }
    public void OnFireInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (buildMode) { if (destroyMode) DestroyObject(); else BuildObject(); }
    }
    public void OnCycleBuildingBlocks(InputAction.CallbackContext ctx)
    {
        if (!(ctx.performed && buildMode) || destroyMode) return;
        chosenBuildingBlockIndex += (ctx.ReadValue<float>() > 0 ? 1 : -1) + buildingListLength;
        chosenBuildingBlockIndex %= buildingListLength;
        SetGhostObjectMesh(chosenBuildingBlockIndex);
    }
    public void OnCycleBuildingRotation(InputAction.CallbackContext ctx)
    {
        if (!(ctx.performed && buildMode) || destroyMode) return;
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
        ghostRenderer = ghostObject.GetComponent<Renderer>();
        SetGhostObjectMesh(chosenBuildingBlockIndex);
        SwitchSquare(true);
    }
    void ExitBuildMode()
    {
        cameraScript.SetBuildCameraEnabled(false);
        buildMode = false;
        destroyMode = false;
        Destroy(ghostObject);
        ghostObject = null;
    }
    void SwitchDestroyMode(bool active)
    {
        if (destroyMode)
        {
            ClearGhostObjectMesh();
            ghostRenderer.material = ghostObjectMaterials[2];
        }
        else
        {
            ghostRenderer.material = ghostObjectMaterials[Convert.ToInt32(!squareInArea())];
            SetGhostObjectMesh(chosenBuildingBlockIndex);
        }
        SwitchSquare(true);
    }
    void SetGhostObjectMesh(int prefabIndex)
    {
        GameObject chosenPrefab = buildingBlocks[prefabIndex];
        ghostMeshFilter.sharedMesh = chosenPrefab.GetComponent<MeshFilter>().sharedMesh;
        Vector3 newScale = chosenPrefab.transform.localScale + Vector3.one * 0.01f;
        ghostObject.transform.localScale = newScale;
        Vector3 newPos = ghostObject.transform.position;
        newPos.y = buildHeight + newScale.y / 2;
        ghostObject.transform.position = newPos;
    }
    void ClearGhostObjectMesh()
    {
        ghostMeshFilter.sharedMesh = null;
    }
    void BuildObject()
    {
        if (!canBuildHere) return;
        GameObject newObject = Instantiate(buildingBlocks[chosenBuildingBlockIndex], 
            ghostObject.transform.position-Vector3.up*0.01f, ghostObject.transform.rotation);
        builtObjectsMatrix[squareIndex.x, squareIndex.y] = newObject;
        prefabIndexMatrix[squareIndex.x, squareIndex.y] = chosenBuildingBlockIndex;
        SwitchSquare(true);
    }
    void DestroyObject()
    {
        Destroy(builtObjectsMatrix[squareIndex.x, squareIndex.y]);
        builtObjectsMatrix[squareIndex.x, squareIndex.y] = null;
        prefabIndexMatrix[squareIndex.x, squareIndex.y] = -1;
        SwitchSquare(true);
    }
    bool squareInArea()
    {
        return squareIndex.x < buildAreaDimensions.x && squareIndex.x >= 0
            && squareIndex.y < buildAreaDimensions.y && squareIndex.y >= 0;
    }
}
