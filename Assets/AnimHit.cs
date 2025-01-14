using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimHit : StateMachineBehaviour
{

    [SerializeField] private float _maxTime = 1;

    private float _timer;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetLayerWeight(1,1);
        _timer = 0;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _timer += Time.deltaTime;
        if (_timer >= _maxTime) {
            animator.SetLayerWeight(1, Mathf.Clamp01(animator.GetLayerWeight(1) - Time.deltaTime));
        }
        else {
            animator.SetLayerWeight(1,1);
        }
    }

    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetLayerWeight(1,0);
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
