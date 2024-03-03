using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Windows;

public class Movement : MonoBehaviour
{
    Transform cam;
    CharacterController cc;
    [SerializeField] GameObject playerObject;
    Animator anim;
    LockOn lockOnScript;
    CameraBeh camBehScript;

    Vector2 input;
    public Vector2 moveDir2;
    public Vector3 moveDir3;

    [SerializeField] bool walkInputting, onGroundLastFrame, running;
    public bool canMoveDeprecated = true;
    bool canMoveByPlayerInput = true;
    float ySpeed;

    [Min(0f)] public float baseMoveSpeed, runMultiplier, baseJumpHeight, playerGravity, terminalVelocity;
    [Range(0,1)] public float turnLerpSpeed;

    bool attacking;

    BuildAWall buildScript;
    public float attackLungeDistance = 1f;
    float attackMoveSpeed;
    float attackLungeAcceleration = 20;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main.transform;
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        buildScript = GetComponent<BuildAWall>();
        lockOnScript = GetComponent<LockOn>();
        camBehScript = GetComponent<CameraBeh>();
    }
    void Update()
    {
        UpdateHorizontalMoveDir();
        CCMove();
        AnimUpdate();
    }

    void AnimUpdate()
    {
        anim.SetBool("running", running);
        if (running && walkInputting && !attacking)
        {
            playerObject.transform.localRotation = Quaternion.Lerp(playerObject.transform.localRotation, 
                (Quaternion.Euler(transform.localRotation.x - 30f, 
                transform.localRotation.y - 180f, transform.localRotation.z)), 15 * Time.deltaTime);
        }
        else
        {
            playerObject.transform.localRotation = Quaternion.Lerp(playerObject.transform.localRotation, Quaternion.Euler(0f, 180f, 0f), 15 * Time.deltaTime);
            //playerObject.transform.localRotation = Quaternion.Euler(0f, transform.localRotation.y - 180f, transform.localRotation.z);
        }

        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);
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
        Vector3 movementVector;
        if (attacking)
        {
            attackMoveSpeed -= attackLungeAcceleration * Time.deltaTime;
            if (attackMoveSpeed <= 0)
            {
                attacking = false;
                canMoveByPlayerInput = true;
            }
            movementVector = transform.forward * attackMoveSpeed;
        }
        else
        {
            float moveSpeed = baseMoveSpeed;
            if (running) moveSpeed *= runMultiplier;

            moveDir3 *= moveSpeed;
            moveDir3.y = ySpeed;

            movementVector = new Vector3(moveDir2.x, 0, moveDir2.y) * moveSpeed;
        }
        movementVector.y = ySpeed;
        cc.Move(movementVector*Time.deltaTime);
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
        if (!canMoveDeprecated) return;
        print(cc.isGrounded);
        if (!(ctx.performed && cc.isGrounded)) return;
        anim.Play("Jump");
        ySpeed = Mathf.Sqrt(2 * playerGravity * baseJumpHeight);
    }
    public void OnRun(InputAction.CallbackContext ctx)
    {
        running = ctx.performed && canMoveDeprecated;
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
            if (lockOnScript.lockedOn && !running)
            {
                transform.rotation = Quaternion.Euler(0, Mathf.LerpAngle(transform.rotation.eulerAngles.y,
                    camBehScript.lockOnAngle, turnLerpSpeed * 60f * Time.deltaTime), 0);
            }
            else if (!attacking)
            {
                transform.rotation = Quaternion.Euler(0, Mathf.LerpAngle(transform.rotation.eulerAngles.y,
                    90 - moveAngle, turnLerpSpeed * 60f * Time.deltaTime), 0);
            }
            
            anim.SetBool("isMoving", true);
        }
        else
        {
            moveDir2 = Vector2.zero;
            moveDir3 = Vector3.zero;
            anim.SetBool("isMoving", false);
        }
    }
    public void OnFireInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || buildScript.buildMode) return;
        Attack();
    }
    void Attack()
    {
        anim.SetTrigger("Attack");
    }
    public void StartAttackLunge()
    {
        canMoveByPlayerInput = false;
        attacking = true;
        float distance = attackLungeDistance * (anim.GetCurrentAnimatorStateInfo(0).IsName("Hit2") ? 1.5f : 1);
        attackMoveSpeed = Mathf.Sqrt(2 * attackLungeAcceleration * distance);
    }
}
