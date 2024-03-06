using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    bool paused;
    Transform pauseMenu;
    GameObject player;
    PlayerInput input;
    private void Start()
    {
        pauseMenu = transform.GetChild(1);
        player = GameObject.Find("Player");
        input = player.GetComponent<PlayerInput>();
    }
    public void OnPause(InputAction.CallbackContext ctx)
   {
        if (!ctx.performed) return;
        if (!paused)
        {
            PauseOn();
        }
        else
        {
            UnPause();
        }
    }
    public void PauseOn()
    {
        paused = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        input.enabled = false;
        pauseMenu.gameObject.SetActive(true);
    }
    public void UnPause()
    {
        paused = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        input.enabled = true;
        pauseMenu.gameObject.SetActive(false);
    }
}
