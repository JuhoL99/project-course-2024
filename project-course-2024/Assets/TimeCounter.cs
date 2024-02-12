using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private WorldTime timer;

    private void Start()
    {
        textBox.text = "";
        timer = WorldTime.instance;
    }
    private void Update()
    {
        string timerString = string.Format("{0:00}:{1:00}:{2:00}", timer.hour, timer.minute, timer.second);
        textBox.text = timerString;
        if(timer.currentTimeOfDay == TimeOfDay.Night)
        {
            textBox.color = Color.red;
            return;
        }
        else
            textBox.color = Color.green;
    }
}
