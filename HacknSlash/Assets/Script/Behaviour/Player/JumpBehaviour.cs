using UnityEngine;

public class JumpBehaviour : PlayerSMB
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
        Player.Instance.m_InputReader.onJump += SetJumpOff;


        animator.SetFloat("DirectionValue", m_dierction);
      
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsGround", Player.Instance.OnGround);
        animator.SetBool("IsJumping",Player.Instance.m_InputReader.IsJumpPressed());
        animator.SetFloat("DirectionValue", m_dierction);
        Player.Instance.m_Rigidbody2D.gravityScale = 4;
        if (animator.GetBool("IsGround"))
        {
            Vector2 vel = Player.Instance.m_Rigidbody2D.linearVelocity;
            // 그냥 Y속도를 20으로 설정해버림. 더도 말고 덜도 말고 딱 20.
            Player.Instance.m_Rigidbody2D.linearVelocity = new Vector2(vel.x, 20f);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance.m_InputReader.onAttack -= SetAttackON;
        Player.Instance.m_InputReader.onMove -= SetDirection;
        Player.Instance.m_InputReader.onMove -= SetMoveON;
        Player.Instance.m_InputReader.offMove -= SetMoveOff;
        Player.Instance.m_InputReader.onJump -= SetJumpOff;

    }

   
}
