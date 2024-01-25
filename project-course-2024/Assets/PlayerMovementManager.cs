using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovementManager : MonoBehaviour
{
    [SerializeField] private PlayerManager player;

    [Header("MOVEMENT INPUTS")]
    public float verticalMovement;
    public float horizontalMovement;
    [SerializeField] private Vector3 moveDirection;

    [SerializeField] private float moveSpeed = 3;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
    }
    void Start()
    {
        
    }
    void Update()
    {
        GroundMovement();
    }
    private void GetMovementInputs()
    {
        verticalMovement = InputManager.instance.verticalInput;
        horizontalMovement = InputManager.instance.horizontalInput;
    }
    private void GroundMovement()
    {
        GetMovementInputs();
        moveDirection = Vector3.forward * verticalMovement;
        moveDirection = moveDirection + Vector3.right * horizontalMovement;
        moveDirection.Normalize();

        player.characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
}
