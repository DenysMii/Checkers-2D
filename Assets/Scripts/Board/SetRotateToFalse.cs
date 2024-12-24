using UnityEngine;

public class SetRotateToFalse : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Rotate", false);
    }
}
