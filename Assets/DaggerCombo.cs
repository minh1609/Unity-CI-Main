using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerCombo : StateMachineBehaviour
{
    PlayerController player;
    Rigidbody2D rb;
    private bool pressedSmash;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pressedSmash = false;
        player = animator.GetComponent<PlayerController>();
        rb = animator.GetComponent<Rigidbody2D>();
        animator.SetBool("attacking", false);
        FindObjectOfType<AudioManager>().Play("dagger");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = Vector2.zero;
        if (Input.GetButton("attack"))
        {
            animator.SetTrigger("Smash");
            pressedSmash = true;
            animator.SetBool("attacking", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Smash");
        if (player.currentState != PlayerState.stagger && player.currentState != PlayerState.interact && !pressedSmash)
        {
            player.currentState = PlayerState.idle;
        }
    }

}
