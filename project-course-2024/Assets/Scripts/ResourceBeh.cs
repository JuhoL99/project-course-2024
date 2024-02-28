using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceBeh : MonoBehaviour, InteractInterface
{
    public string resourceName;
    void Start() { 
    }
    public void GetInteracted(GameObject player)
    {
        PlayerManager manager = player.GetComponent<PlayerManager>();
        manager.currentResources[manager.nameToResourceNum[resourceName]]++;
        print(manager.currentResources.ToCommaSeparatedString());
        player.GetComponentInChildren<InteractVolume>().interactablesInVolume.Remove(gameObject); 
        Destroy(gameObject);
    }
}
