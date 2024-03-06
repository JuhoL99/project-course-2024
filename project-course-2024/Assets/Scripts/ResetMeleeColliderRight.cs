using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMeleeColliderRight : StateMachineBehaviour
{
    public float resetPercent;
    bool colliderReset;
    //nStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerWeapon fist = GameObject.FindWithTag("FistRight").GetComponent<PlayerWeapon>();
        fist.EnableWeapon(true);
        colliderReset = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!colliderReset && stateInfo.normalizedTime > resetPercent)
        {
            colliderReset = true;
            PlayerWeapon fist = GameObject.FindWithTag("FistRight").GetComponent<PlayerWeapon>();
            fist.EnableWeapon(false);
            Debug.Log("resetRight");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        /*GameObject playerObject = GameObject.Find("Player");
        if(playerObject != null)
        {
            Movement moveScript = playerObject.GetComponent<Movement>();
            if(moveScript != null)
            {
                moveScript.canMoveDeprecated = true;
            }
        }*/
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
