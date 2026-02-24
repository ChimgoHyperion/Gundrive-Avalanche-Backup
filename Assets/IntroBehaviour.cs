using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBehaviour : StateMachineBehaviour
{
    private int rand;
    Transform PlayerPosition;
    public float speed;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // for moving toward the player
        PlayerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        // as this animation is playing movetoward player

        Vector2 target = new Vector2(PlayerPosition.position.x, animator.transform.position.y);
        animator.transform.position = Vector2.MoveTowards(animator.transform.position, target, speed * Time.deltaTime);
        // random animations playing 
        rand = Random.Range(0, 2);
        if (rand == 0)
        {
          //  animator.SetTrigger("Jump");
        }
        else
        {
          //  animator.SetTrigger("FirstAttack");
        }
       
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
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
