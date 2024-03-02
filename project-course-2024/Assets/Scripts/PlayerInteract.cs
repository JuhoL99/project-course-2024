using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    InteractVolume interactVolume;
    void Start()
    {
        interactVolume = GetComponentInChildren<InteractVolume>();
    }

    void Update()
    {
        
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!(ctx.performed && interactVolume.interactableInRange)) return;
        RemoveNullsFromRange();
        if (!interactVolume.interactableInRange) return;
        InteractWithClosestInteractable();
    }
    void RemoveNullsFromRange()
    {
        GameObject[] interactables = interactVolume.interactablesInVolume.ToArray();
        foreach (GameObject go in interactables)
        {
            if (go == null)
            {
                interactVolume.RemoveInteractableFromRange(go);
            }
        }
    }
    void InteractWithClosestInteractable()
    {
        GameObject[] interactables = interactVolume.interactablesInVolume.ToArray();
        //Find closest interactable
        GameObject closestInteractable = FindClosestInteractable(interactables);
        //Get component that inherits InteractInterface and call GetInteracted on it
        closestInteractable.GetComponent<InteractInterface>().GetInteracted(gameObject);
    }

    GameObject FindClosestInteractable(GameObject[] objects)
    {
        Vector3 playerPos = transform.position;
        int shortestDistanceIdx = 0;
        float shortestDistance = (objects[0].transform.position - playerPos).magnitude;
        for (int i = 0; i < objects.Length; i++)
        {
            float distance = (objects[i].transform.position - playerPos).magnitude;
            if (distance < shortestDistance)
            {
                shortestDistanceIdx = i;
                shortestDistance = distance;
            }
        }
        return objects[shortestDistanceIdx];
    }
}
