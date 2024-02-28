using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnVolume : MonoBehaviour
{
    public List<GameObject> enemiesInVolume = new List<GameObject>();
    private void OnTriggerEnter(Collider other)
    {
        enemiesInVolume.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        enemiesInVolume.Remove(other.gameObject);
    }
}
