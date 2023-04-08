using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeOpener : StateMachineBehaviour
{
    PlayerController player;
    Rigidbody2D rb;
    private bool pressedDagger;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FindObjectOfType<AudioManager>().Play("Normal Sword Swing");
        pressedDagger = false;
        player = animator.GetComponent<PlayerController>();
        rb = animator.GetComponent<Rigidbody2D>();
        animator.ResetTrigger("meleeOpener");
        animator.SetBool("attacking", false);
        rb.velocity = Vector2.zero;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetKeyDown(KeyCode.V) && player.daggerUsable())
        {
            animator.SetTrigger("DaggerCombo");
            player.daggerUsed();
            pressedDagger = true;
            animator.SetBool("attacking", true);
        }
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("DaggerCombo");
        if (player.currentState != PlayerState.stagger && player.currentState != PlayerState.interact && !pressedDagger)
        {
            player.currentState = PlayerState.idle;
        }
    }

}
