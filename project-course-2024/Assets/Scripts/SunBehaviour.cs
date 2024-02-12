using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunBehaviour : MonoBehaviour
{
    [SerializeField] private Light mainLight;
    [SerializeField] private WorldTime timer;
    [SerializeField] private Gradient lightIntensity; //gradient for light intensity based on time
    [SerializeField] private Gradient ambientColor; //gradient for environment tint based on current time
    [SerializeField] private Gradient lightColor;

    void Start()
    {
        timer = GetComponent<WorldTime>();
        mainLight = GameObject.FindWithTag("MainLight").GetComponent<Light>(); //temporary
    }
    void Update()
    {
        UpdateLight();

    }
    private void UpdateLight()
    {
        if (mainLight != null)
        {
            RotateLight();
            ChangeLightIntensity();
        }

    }
    private void RotateLight()
    {
        mainLight.transform.localRotation = Quaternion.Euler(new Vector3((timer.timeFraction * -360) - 90, 90, 90)); //rotate sunlight from east to west
    }
    private void ChangeLightIntensity() //temp for testing, add rotating skybox or something?
    {
        RenderSettings.ambientLight = ambientColor.Evaluate(timer.timeFraction);
        mainLight.intensity = lightIntensity.Evaluate(timer.timeFraction).grayscale;
        mainLight.color = lightColor.Evaluate(timer.timeFraction);
    }
}
