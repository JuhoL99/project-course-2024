using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
