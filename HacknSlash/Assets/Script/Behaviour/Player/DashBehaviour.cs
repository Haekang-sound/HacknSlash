using UnityEngine;

public class DashBehaviour : PlayerSMB
{
    float dir = 0;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance.m_Animator.SetBool("IsCrouch", false);
        Player.Instance.m_damageable.m_isInvincible = true;
        m_dierction = Player.Instance.UnitDirection.x;
        dir = m_dierction;
        animator.SetFloat("DirectionValue", m_dierction);
        Player.Instance.m_InputReader.onAttack += SetAttackON;
        Player.Instance.m_InputReader.onMove += SetMoveON;
        Player.Instance.m_InputReader.offMove += SetMoveOff;
        Player.Instance.m_InputReader.onJump += SetJumpON;
        Player.Instance.m_InputReader.offCrouch += SetCrouchOff;

        Player.Instance.m_Rigidbody2D.linearVelocity = new Vector2(Player.Instance.m_Rigidbody2D.linearVelocityX, 0f);

        //// 2. 위쪽으로 순간적인 힘 가하기 (Impulse 모드 사용)
        //if (Player.Instance.OnGround)
        //{
        //    Player.Instance.m_Rigidbody2D.AddForce(
        //        Vector2.right * m_dierction
        //        * dashPower, ForceMode2D.Impulse);
        //}
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("IsGround"))
        {
            Vector2 vel = Player.Instance.m_Rigidbody2D.linearVelocity;
            // 그냥 Y속도를 20으로 설정해버림. 더도 말고 덜도 말고 딱 20.
            Player.Instance.m_Rigidbody2D.linearVelocityX = 16 * dir;
        }
        DeAccelerate();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance.m_damageable.m_isInvincible = false;
        Player.Instance.m_Animator.SetBool("IsCrouch", false);
        Player.Instance.m_InputReader.onAttack -= SetAttackON;
        Player.Instance.m_InputReader.onMove -= SetMoveON;
        Player.Instance.m_InputReader.offMove -= SetMoveOff;
        Player.Instance.m_InputReader.onJump -= SetJumpON;
        Player.Instance.m_InputReader.offCrouch -= SetCrouchOff;
    }

}
