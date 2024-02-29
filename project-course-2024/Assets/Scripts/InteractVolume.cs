using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractVolume : MonoBehaviour
{
    public List<GameObject> interactablesInVolume = new List<GameObject>();
    public bool interactableInRange;
    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if (other.gameObject.TryGetComponent(out InteractInterface component))
        {
            interactablesInVolume.Add(other.gameObject);
            interactableInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        interactablesInVolume.Remove(other.gameObject);
        if (interactablesInVolume.Count == 0) interactableInRange = false;
    }
    public void RemoveInteractableFromRange(GameObject interactable)
    {
        interactablesInVolume.Remove(interactable);
        if (interactablesInVolume.Count == 0) interactableInRange = false;
    }
}
