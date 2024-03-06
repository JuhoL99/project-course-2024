using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceBeh : MonoBehaviour, InteractInterface
{
    public string resourceName;
    [System.NonSerialized] public ResourceSpawner spawnerScript;
    void Start() { 
    }
    public void GetInteracted(GameObject player)
    {
        PlayerManager manager = player.GetComponent<PlayerManager>();
        manager.currentResources[manager.nameToResourceNum[resourceName]]+=10;
        print(manager.currentResources.ToCommaSeparatedString());
        player.GetComponentInChildren<InteractVolume>().RemoveInteractableFromRange(gameObject);
        spawnerScript.worldResourceCount--;
        Destroy(gameObject);
    }
}
