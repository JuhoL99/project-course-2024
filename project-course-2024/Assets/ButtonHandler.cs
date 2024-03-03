using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    UIManager uiManager;
    private void Start()
    {
        uiManager = UIManager.instance;
    }
    public void MainMenu()
    {
        uiManager.MainMenu();
    }
}
