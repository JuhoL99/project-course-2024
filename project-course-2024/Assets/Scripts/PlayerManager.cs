using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Dictionary<string,int> nameToResourceNum;
    public int[] currentResources;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        CreateResources();
    }
    void CreateResources()
    {
        nameToResourceNum = new Dictionary<string,int>
        {
            { "Stone", 0 },
            { "Iron", 1 },
            { "Wood", 2 },
            { "Egg Food", 3 }
        };
        currentResources = new int[nameToResourceNum.Count];
    }
    public void OnResourceCheat(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        for (int i = 0; i < currentResources.Length; i++)
        {
            currentResources[i] += 10;
        }
        print(currentResources.ToCommaSeparatedString());
    }
    public int GetResourceAmount(string resourceName)
    {
        if(nameToResourceNum.ContainsKey(resourceName))
        {
            int n = nameToResourceNum[resourceName];
            return currentResources[n];
        }
        return -1;
    }
}
