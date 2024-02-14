using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TimeOfDay
{
    Day,
    Night
}
public class WorldTime : MonoBehaviour
{
    public TimeOfDay currentTimeOfDay;
    public static WorldTime instance;
    [SerializeField] private float currentTime;
    [SerializeField, Range(0, 24)] private float timeOfDay;
    [SerializeField] private int daysPassed = 0; //How many days have passed in game
    public float timeFraction;

    [Header("Length of ingame day (seconds)")]
    public int dayLength = 600; //Length of ingame day in seconds

    [Header("Hours, minutes, seconds")]
    public int hour;
    public int minute;
    public int second;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        currentTime = dayLength / 2; // Start at noon
    }
    private void Update()
    {
        if (hour >= 18 || hour <= 6)
        {
            currentTimeOfDay = TimeOfDay.Night;
        }
        else
            currentTimeOfDay = TimeOfDay.Day;
        if (currentTime > dayLength)
        {
            currentTime = 0;
            daysPassed++;
        }
        currentTime += Time.deltaTime;
        timeFraction = currentTime / dayLength;
        TimeConversion(timeFraction);
        timeOfDay = Mathf.Clamp01(currentTime / dayLength) * 24;
    }
    private void TimeConversion(float timeFraction)
    {
        float secondsPassed = timeFraction * 24 * 60 * 60;
        hour = (int)(secondsPassed / 3600);
        minute = (int)((secondsPassed % 3600) / 60);
        second = (int)(secondsPassed % 60);
    }
}
