using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshManager : MonoBehaviour
{
    public static NavMeshManager instance;
    [SerializeField] NavMeshSurface terrain;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void RebakeNavMesh()
    {
        terrain.BuildNavMesh();
    }
}
