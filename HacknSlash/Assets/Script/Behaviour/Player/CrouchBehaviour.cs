using UnityEngine;
using UnityEngine.UIElements;

public class CrouchBehaviour : PlayerSMB
{
    //private float m_dierction = 0;

    //public float maxSpeed = 10f; // 원하는 최대 속도 값 설정
    //public float accelerationPower = 4f; // 가속 힘
    //public float decelerationSpeed = 50f; // 감속하는 힘의 크기

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_dierction = Player.Instance.UnitDirection.x;
        animator.SetFloat("DirectionValue", m_dierction);
        Player.Instance.m_InputReader.onAttack += SetAttackON;
        Player.Instance.m_InputReader.onMove += SetMoveON;
        Player.Instance.m_InputReader.offMove += SetMoveOff;
        Player.Instance.m_InputReader.onJump += SetJumpON;
        Player.Instance.m_InputReader.offCrouch += SetCrouchOff;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("DirectionValue", m_dierction);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player.Instance.m_Animator.SetBool("IsCrouch", false);
        Player.Instance.m_InputReader.onAttack -= SetAttackON;
        Player.Instance.m_InputReader.onMove -= SetMoveON;
        Player.Instance.m_InputReader.offMove -= SetMoveOff;
        Player.Instance.m_InputReader.onJump -= SetJumpON;
        Player.Instance.m_InputReader.offCrouch -= SetCrouchOff;
    }

    //public void SetMoveON()
    //{
    //    SetDirection();
    //    Player.Instance.m_Animator.SetBool("IsMove", true);
    //}
    //public void SetMoveOff()
    //{
    //    Player.Instance.m_Animator.SetBool("IsMove", false);
    //}
    //public void SetJumpON()
    //{
    //    Player.Instance.m_Animator.SetBool("IsJumping", true);
    //    Player.Instance.m_Animator.SetBool("IsGround", false);
    //}

    //public void SetDirection()
    //{
    //    m_dierction = Player.Instance.UnitDirection.x;
    //}
   
    //public void SetCrouchOff()
    //{
    //    Player.Instance.m_Animator.SetBool("IsCrouch", false);
    //}
    //public void SetAttackON()
    //{
    //    Player.Instance.m_Animator.SetTrigger("OnAttack");
    //}
}

