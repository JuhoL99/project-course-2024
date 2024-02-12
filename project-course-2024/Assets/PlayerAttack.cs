using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private bool isAttacking;
    private void Update()
    {

    }
    public void OnFireInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        PerformAttack();
    }
    private void PerformAttack()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.red, 20f);
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
