using UnityEngine;

public class AirattackBehaviour : PlayerSMB
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance.ShakeON();
        // 4. 방향 벡터 계산 (목표 - 내위치)
        Vector2 direction = Player.Instance.CalculateMouseDirection();
        m_dierction = Player.Instance.CalculateMouseDirection().x;
        Player.Instance.UnitDirection = direction.normalized;
        animator.SetFloat("DirectionValue", m_dierction);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance.ShakeON();
        // 4. 방향 벡터 계산 (목표 - 내위치)
        Vector2 direction = Player.Instance.CalculateMouseDirection();
        m_dierction = Player.Instance.CalculateMouseDirection().x;
        Player.Instance.UnitDirection = direction.normalized;
        animator.SetFloat("DirectionValue", m_dierction);
        // 마우스 방향으로 가속한다.
        Player.Instance.m_Rigidbody2D.linearVelocity = Vector2.zero;

        Player.Instance.m_Rigidbody2D.AddForce(
            direction.normalized
            * dashPower, ForceMode2D.Impulse);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsJumping", false);
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
