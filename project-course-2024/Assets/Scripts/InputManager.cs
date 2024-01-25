using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public PlayerManager playerManager;
    PlayerControls playerControls;

    [Header("Movement Inputs")]
    [SerializeField] private Vector2 movementVector;
    public float horizontalInput;
    public float verticalInput;


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
    private void Update()
    {
        MovementInput();
    }
    void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.PlayerMovement.Movement.performed += i => movementVector = i.ReadValue<Vector2>();
        }
        playerControls.Enable();
    }
    private void MovementInput()
    {
        verticalInput = movementVector.y;
        horizontalInput = movementVector.x;
    }
}
