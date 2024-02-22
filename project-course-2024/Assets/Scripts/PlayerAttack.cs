using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private int currentComboPhase = 0;
    private float lastInputTime;
    private float comboTimer = 1f;
    private bool isAttacking;
    [SerializeField] private AnimationClip[] comboSteps;
    [SerializeField] private Animator playerAnim;


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
        Debug.Log("test");
        playerAnim.Play("Hit1");
        //if new attack within combo time, advance phase
        //Debug.Log(currentComboPhase);
        /*lastInputTime = Time.time;
        Debug.Log(lastInputTime);
        if(Time.time - lastInputTime > comboTimer)
        {
            //Debug.Log(Time.time - lastInputTime);
            currentComboPhase = 0;
            return;
        }
        if(currentComboPhase < comboSteps.Length)
        {
            currentComboPhase++;
        }
        else
        {
            currentComboPhase = 0;
        }*/



        /*RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.red, 20f);
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Destroy(hit.collider.gameObject);
            }
            else if(hit.collider.gameObject.layer == LayerMask.NameToLayer(""))
            {

            }
        }*/
    }
}
