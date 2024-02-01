using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildAWall : MonoBehaviour
{
    public GameObject aWall;
    GameObject ghostObject;
    void Start()
    {
    }

    void Update()
    {
    }
    public void OnBuildInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        BuildWall();
    }
    void BuildWall()
    {
        Vector3 buildPos = transform.position + transform.forward * 2f;
        Vector3 buildRot = transform.rotation.eulerAngles;
        buildPos.y = aWall.transform.localScale.y/2;
        buildRot.y = Mathf.FloorToInt((buildRot.y+45) / 90f) * 90f;
        GameObject newWall = Instantiate(aWall, buildPos, Quaternion.Euler(buildRot));
    }
}
