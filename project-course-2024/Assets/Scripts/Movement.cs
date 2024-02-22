using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Movement : MonoBehaviour
{
    Transform cam;
    CharacterController cc;

    Vector2 input;
    Vector2 moveDir2;
    Vector3 moveDir3;

    bool walkInputting, onGroundLastFrame, running;
    float ySpeed;

    [Min(0f)] public float baseMoveSpeed, runMultiplier, baseJumpHeight, playerGravity, terminalVelocity;
    [Range(0,1)] public float turnLerpSpeed;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main.transform;
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        UpdateHorizontalMoveDir();
        CCMove();
    }
    void FixedUpdate()
    {
    }
    void CCMove()
    {
        if (!cc.isGrounded)
        {
            ySpeed -= playerGravity * Time.deltaTime;
        }
        else if (!onGroundLastFrame && cc.isGrounded)
        {
            ySpeed = -0.01f;
        }
        float moveSpeed = baseMoveSpeed;
        if (running) moveSpeed *= runMultiplier;
        moveDir3 *= moveSpeed;
        moveDir3.y = ySpeed;
        cc.Move(new Vector3(moveDir2.x*moveSpeed,ySpeed,moveDir2.y*moveSpeed)  * Time.deltaTime);
        onGroundLastFrame = cc.isGrounded;
    }
    public void OnWalk(InputAction.CallbackContext ctx)
    {
        input = ctx.ReadValue<Vector2>();
        if (input.magnitude < 0.01f)
        {
            walkInputting = false;
            return;
        }
        walkInputting = true;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!(ctx.performed && cc.isGrounded)) return;
        ySpeed = Mathf.Sqrt(2 * playerGravity * baseJumpHeight);
    }
    public void OnRun(InputAction.CallbackContext ctx)
    {
        running = ctx.performed;
    }
    void UpdateHorizontalMoveDir()
    {
        if (walkInputting)
        {
            float inputAngle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
            float camAngle = -cam.rotation.eulerAngles.y;
            float moveAngle = (inputAngle + camAngle) % 360;
            moveDir2 = new Vector2(Mathf.Cos(moveAngle * Mathf.Deg2Rad), Mathf.Sin(moveAngle * Mathf.Deg2Rad));
            moveDir3 = new Vector3(moveDir2.x, 0f, moveDir2.y);
            transform.rotation = Quaternion.Euler(0, Mathf.LerpAngle(transform.rotation.eulerAngles.y, 90 - moveAngle, turnLerpSpeed*60f*Time.deltaTime), 0);
        }
        else
        {
            moveDir2 = Vector2.zero;
            moveDir3 = Vector3.zero;
        }
    }
}
