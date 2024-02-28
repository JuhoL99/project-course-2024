using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractVolume : MonoBehaviour
{
    public List<GameObject> objectsInVolume = new List<GameObject>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Interactable interactableScript))
        objectsInVolume.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        objectsInVolume.Remove(other.gameObject);
    }
}
