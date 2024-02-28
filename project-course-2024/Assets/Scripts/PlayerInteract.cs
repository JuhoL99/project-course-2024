using System.Collections;
using System.Collections.Generic;
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
        GameObject[] interactables = interactVolume.interactablesInVolume.ToArray();
        Vector3 playerPos = transform.position;
        int shortestDistanceIdx = 0;
        float shortestDistance = (interactables[0].transform.position - playerPos).magnitude;
        for (int i = 0; i < interactables.Length; i++)
        {
            float distance = (interactables[0].transform.position - playerPos).magnitude;
            if (distance < shortestDistance)
            {
                shortestDistanceIdx = i;
                shortestDistance = distance;
            }
        }
        interactables[shortestDistanceIdx].GetComponent<InteractInterface>().GetInteracted(gameObject);
    }
}
