using UnityEngine;

public class LandingBehaviour : PlayerSMB
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_dierction = Player.Instance.UnitDirection.x;
        animator.SetFloat("DirectionValue", m_dierction);
        Player.Instance.m_InputReader.onAttack += SetAttackON;
        Player.Instance.m_InputReader.onMove += SetDirection;
        Player.Instance.m_InputReader.onMove += SetMoveON;
        Player.Instance.m_InputReader.offMove += SetMoveOff;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance.m_Rigidbody2D.gravityScale = 2;

        Player.Instance.m_Rigidbody2D.AddForceY(-12);
        if(Player.Instance.m_Rigidbody2D.linearVelocityY > 20f)
        {
            Player.Instance.m_Rigidbody2D.linearVelocityY = 20f;
        }
        animator.SetFloat("DirectionValue", m_dierction);
        if (animator.GetBool("IsMove"))
        {
            Accelerate();
        }
        else
        {
            //DeAccelerate();
        }

        //  상승중이 아니면 점프를 종료하고 하강으로 전환한다.
        if (Player.Instance.OnGround)
        {
            OnLanded();
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance.m_Rigidbody2D.gravityScale = 1;
        Player.Instance.m_InputReader.onAttack -= SetAttackON;
        Player.Instance.m_InputReader.onMove -= SetDirection;
        Player.Instance.m_InputReader.onMove -= SetMoveON;
        Player.Instance.m_InputReader.offMove -= SetMoveOff;

    }

}
