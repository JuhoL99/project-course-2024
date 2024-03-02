using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private float lastInputTime;
    private float comboTimer = 1f;
    public bool isAttacking = false;
    [SerializeField] private string[] comboSteps;
    [SerializeField] private int currentComboPhase = 0;
    [SerializeField] private Animator playerAnim;
    BuildAWall builder;
    private Movement movementScript;

    private void Start()
    {
        builder = GetComponent<BuildAWall>();
        movementScript = GetComponent<Movement>();
    }
    private void Update()
    {

    }
    public void OnFireInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed || builder.buildMode) return;
        PerformAttack();
    }
    private void PerformAttack()
    {
        Debug.Log("test");
        if (currentComboPhase > 2)
        {
            currentComboPhase = 0;
        }
        movementScript.canMove = false;
        if(!isAttacking)
        {
            StartCoroutine(AttackAnimation());
            currentComboPhase++;
        }

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
    private IEnumerator AttackAnimation()
    {
        isAttacking = true;
        playerAnim.Play(comboSteps[currentComboPhase]);
        yield return new WaitForSeconds(playerAnim.GetCurrentAnimatorStateInfo(0).length);
        isAttacking = false;
    }
}
