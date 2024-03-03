using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    [Range(0, 1)] public float value;
    RectTransform fill;
    void Start()
    {
        fill = transform.GetChild(1).GetComponent<RectTransform>();
    }
    void OnValidate()
    {
        fill = transform.GetChild(1).GetComponent<RectTransform>();
        fill.localScale = new Vector3(value, 1, 1);
    }
    public void UpdateValue(float newValue)
    {
        value = newValue;
        fill.localScale = new Vector3(newValue, 1, 1);
    }
}
