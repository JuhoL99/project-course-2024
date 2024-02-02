using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildAWall : MonoBehaviour
{
    public GameObject[] buildingBlocks;
    public Material buildMaterial;
    public int gridSize;
    GameObject ghostObject;
    Renderer rend;
    bool buildMode;
    int chosenBuildingBlockIndex;
    int buildingListLength;
    void Start()
    {
        buildingListLength = buildingBlocks.Length;
    }

    void Update()
    {
        BuildModeUpdate();
    }
    void BuildModeUpdate()
    {
        if (!buildMode) return;
        Vector3 buildPos = transform.position + transform.forward * 4f;
        Vector3 buildRot = transform.rotation.eulerAngles;
        buildPos.x = Mathf.FloorToInt((buildPos.x + ghostObject.transform.localScale.x/2) / gridSize) * gridSize;
        buildPos.z = Mathf.FloorToInt((buildPos.z + ghostObject.transform.localScale.z/ 2) / gridSize) * gridSize;
        buildPos.y = buildingBlocks[chosenBuildingBlockIndex].transform.localScale.y / 2 + 0.01f;
        buildRot.y = Mathf.FloorToInt((buildRot.y + 45) / 90f) * 90f;
        ghostObject.transform.position = buildPos;
        ghostObject.transform.rotation = Quaternion.Euler(buildRot);
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
            BuildWall();
        }
    }
    public void OnCycleBuildingBlocks(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        chosenBuildingBlockIndex += (ctx.ReadValue<float>() > 0 ? 1 : -1) + buildingBlocks.Length;
        chosenBuildingBlockIndex %= buildingBlocks.Length;
        Destroy(ghostObject);
        MakeGhostObject();
    }
    void EnterBuildMode()
    {
        buildMode = true;
        MakeGhostObject();
    }
    void MakeGhostObject()
    {
        ghostObject = Instantiate(buildingBlocks[chosenBuildingBlockIndex]);
        ghostObject.GetComponent<Renderer>().material = buildMaterial;
        Destroy(ghostObject.GetComponent<Collider>());
    }
    void ExitBuildMode()
    {
        buildMode = false;
        Destroy(ghostObject);
        ghostObject = null;
    }
    void BuildWall()
    {
        GameObject newObject = Instantiate(buildingBlocks[chosenBuildingBlockIndex], ghostObject.transform.position, ghostObject.transform.rotation);
    }
}
