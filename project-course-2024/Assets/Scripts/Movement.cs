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
    [SerializeField] GameObject playerObject;
    [SerializeField] private Animator anim;

    Vector2 input;
    public Vector2 moveDir2;
    public Vector3 moveDir3;

    [SerializeField] bool walkInputting, onGroundLastFrame, running;
    public bool canMove = true;
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
        anim.SetBool("running", running);
        if(running && walkInputting)
        {
            playerObject.transform.localRotation = Quaternion.Lerp(playerObject.transform.localRotation, (Quaternion.Euler(transform.localRotation.x-30f, transform.localRotation.y-180f, transform.localRotation.z)),5*Time.deltaTime);
        }
        else
        {
            playerObject.transform.localRotation = Quaternion.Lerp(playerObject.transform.localRotation, Quaternion.Euler(0f,180f,0f),5*Time.deltaTime);
            //playerObject.transform.localRotation = Quaternion.Euler(0f, transform.localRotation.y - 180f, transform.localRotation.z);
        }

        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);

        UpdateHorizontalMoveDir();
        CCMove();
    }
    void FixedUpdate()
    {
    }
    void CCMove()
    {
        /*if (!canMove)
            return;*/
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
        if (!canMove) moveSpeed *= 0.2f;
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
        anim.Play("Jump");
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
            anim.SetBool("isMoving", true);
        }
        else
        {
            moveDir2 = Vector2.zero;
            moveDir3 = Vector3.zero;
            anim.SetBool("isMoving", false);
        }
    }
}
