using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smash : StateMachineBehaviour
{
    PlayerController player;
    Rigidbody2D rb;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = animator.GetComponent<PlayerController>();
        rb = animator.GetComponent<Rigidbody2D>();
        animator.SetBool("attacking", false);
        FindObjectOfType<AudioManager>().PlayWithSettings("Normal Sword Swing", 0.065f, 0.8f);
        FindObjectOfType<AudioManager>().Play("Rock-smash");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.velocity = Vector2.zero;
        if (animator.GetBool("attacking"))
            animator.SetBool("attacking", false);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player.currentState != PlayerState.stagger && player.currentState != PlayerState.interact)
        {
            player.currentState = PlayerState.idle;
        }
    }


}
