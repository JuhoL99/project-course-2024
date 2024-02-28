using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : MonoBehaviour
{
    [SerializeField] private GameObject gate;
    //closed y = 2 opened y = 5
    [SerializeField] private Vector3 closedPos;
    [SerializeField] private float openHeight = 5;
    [SerializeField] private bool opening;
    [SerializeField] private bool closing;
    public float openingSpeed;


    void Start()
    {
        gate = transform.GetChild(0).gameObject;
        closedPos = gate.transform.localPosition;
    }
    void Update()
    {
        // if interacted
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleGate();
        }
        if (opening)
        {
            gate.transform.localPosition += Vector3.up * openingSpeed * Time.deltaTime;
            if (gate.transform.localPosition.y >= closedPos.y + openHeight)
            {
                opening = false;
            }
            if (!closing && gate.transform.localPosition.y >= closedPos.y + openHeight)
            {
                closing = true;
            }
        }
        else if (closing)
        {
            gate.transform.localPosition -= Vector3.up * openingSpeed * Time.deltaTime;

            if (gate.transform.localPosition.y <= closedPos.y)
            {
                closing = false;
            }
        }
    }
    public void ToggleGate()
    {
        if (opening || closing)
        {
            opening = !opening;
            closing = !closing;
        }
        else
        {
            opening = true;
        }
    }
}
