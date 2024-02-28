using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using NavMeshBuilder = UnityEngine.AI.NavMeshBuilder;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            RebakeNavMesh();
        }
    }

    public void RebakeNavMesh()
    {
        Bounds rebakeBounds = CalculateRebakeBounds();
        NavMeshData navMeshData = NavMeshBuilder.BuildNavMeshData(new NavMeshBuildSettings(), new List<NavMeshBuildSource>(), rebakeBounds, Vector3.zero, Quaternion.identity);
        terrain.navMeshData = navMeshData;
    }

    private Bounds CalculateRebakeBounds()
    {
        Collider rebakeCollider = GameObject.Find("RebakeArea").GetComponent<Collider>();

        if (rebakeCollider != null)
        {
            return rebakeCollider.bounds;
        }
        else
        {
            Debug.Log("noo collider");
            return new Bounds(Vector3.zero, Vector3.zero);
        }
    }
}
