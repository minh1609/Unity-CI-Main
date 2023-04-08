using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : StateMachineBehaviour
{
    private Rigidbody2D rb;
    private PlayerController player;
    private float dashTimer;
    private float dashDuration;
    private Vector2 lastDirection;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        player = animator.GetComponent<PlayerController>();

        Vector2 temp = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (temp == lastDirection && dashTimer >= Time.time && player.playerInventory.currentMagic >= player.dashMana && player.dashUsable())
        {
            player.dashDuration = Time.time + 1f;
            player.startDash(temp);
            FindObjectOfType<AudioManager>().Play("woosh");
        }
        dashTimer = Time.time + 0.3f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        lastDirection = new Vector2(animator.GetFloat("lastMoveX"), animator.GetFloat("lastMoveY"));
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
