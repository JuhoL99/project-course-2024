using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, 10*Time.deltaTime);
    }
}
