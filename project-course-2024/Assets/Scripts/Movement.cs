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
    bool moving;
    Vector2 moveDir2;
    Vector3 moveDir3;
    public float baseMoveSpeed;
    Vector2 input;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main.transform;
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        UpdateMoveDir();
        Move();
    }
    void Move()
    {
        if (!moving) return;
        cc.Move(moveDir3 * baseMoveSpeed * Time.deltaTime);
    }
    public void OnWalk(InputAction.CallbackContext ctx)
    {
        input = ctx.ReadValue<Vector2>();
        if (input.magnitude < 0.01f)
        {
            moving = false;
            return;
        }
        moving = true;
        
    }
    void UpdateMoveDir()
    {
        if (!moving) return;
        float inputAngle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
        float camAngle = -cam.rotation.eulerAngles.y;
        float moveAngle = (inputAngle + camAngle) % 360;
        moveDir2 = new Vector2(Mathf.Cos(moveAngle * Mathf.Deg2Rad), Mathf.Sin(moveAngle * Mathf.Deg2Rad));
        moveDir3 = new Vector3(moveDir2.x, 0f, moveDir2.y);
        transform.rotation = Quaternion.Euler(0, -moveAngle + 180, 0);
    }
}
